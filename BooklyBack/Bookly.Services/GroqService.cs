using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Bookly.Services.DTOs;
using Bookly.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Bookly.Services;

public class GroqService : IGroqService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly JsonSerializerOptions _jsonOptions;

    private const string ApiUrl = "https://api.groq.com/openai/v1/chat/completions";

    private const string SystemPromptChatbot = "Você é o Bookly Bot, um especialista apaixonado por literatura que adora ajudar pessoas a descobrirem o próximo livro favorito. Você conhece milhares de livros de todos os gêneros, épocas e culturas. Seu tom é caloroso, entusiasmado e amigável.\n\nRegras obrigatórias:\n- Sempre retorne entre 5 e 10 livros reais (nunca invente títulos).\n- Nunca repita livros que o usuário já mencionou ter lido.\n- Adapte as recomendações ao pedido: gênero, emoção, ritmo, complexidade, faixa etária.\n- Se o pedido for vago, sugira clássicos variados e populares.\n- Responda SOMENTE com um JSON válido, sem markdown, sem explicações fora do JSON.\n\nFormato de saída (JSON puro):\n{\n  \"mensagem\": \"Texto amigável e entusiasmado explicando as sugestões (2-3 frases)\",\n  \"recomendacoes\": [\n    {\n      \"titulo\": \"Nome do Livro\",\n      \"autor\": \"Nome do Autor\",\n      \"motivo\": \"Motivo curto e empolgante da recomendação\"\n    }\n  ]\n}";

    public GroqService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Groq:ApiKey"] ?? "";

        if (string.IsNullOrWhiteSpace(_apiKey))
        {
            throw new InvalidOperationException("Chave da API Groq não configurada (Groq:ApiKey).");
        }

        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<LivroRecomendadoDto>> RecomendarLivrosAsync(IEnumerable<LivroAvaliadoDto> livrosAvaliados)
    {
        var listaLivros = "";
        foreach (var l in livrosAvaliados)
        {
            if (l.Nota >= 3)
            {
                listaLivros += "- \"" + l.Titulo + "\" de " + l.Autor + " (nota: " + l.Nota + "/5)\n";
            }
        }

        if (listaLivros == "")
        {
            listaLivros = "Nenhum livro avaliado ainda.";
        }

        var prompt = "Você é um especialista em literatura e recomendação de livros.\n\n" + 
                     "O usuário avaliou positivamente os seguintes livros:\n" +
                     listaLivros + "\n" +
                     "Com base nesses gostos, recomende exatamente 10 livros que o usuário ainda não leu.\n\n" +
                     "Regras:\n" +
                     "- NÃO repita nenhum dos livros já listados acima.\n" +
                     "- Priorize semelhanças em: gênero, estilo, temas, público-alvo e tom narrativo.\n" +
                     "- Se houver poucos dados, complete com clássicos populares relacionados.\n" +
                     "- Retorne APENAS um array JSON válido, sem texto adicional, sem markdown.\n\n" +
                     "Formato de saída (JSON puro):\n" +
                     "[\n" +
                     "  {\n" +
                     "    \"titulo\": \"Nome do Livro\",\n" +
                     "    \"autor\": \"Nome do Autor\",\n" +
                     "    \"motivo\": \"Breve explicação da recomendação\"\n" +
                     "  }\n" +
                     "]";

        var rawJson = await EnviarRequisicao(prompt, "Você é um recomendador de livros profissional.");

        var inicio = rawJson.IndexOf('[');
        var fim = rawJson.LastIndexOf(']');
        var jsonLimpo = rawJson.Substring(inicio, fim - inicio + 1);

        var resultado = JsonSerializer.Deserialize<List<LivroRecomendadoDto>>(jsonLimpo, _jsonOptions);
        return resultado ?? new List<LivroRecomendadoDto>();
    }

    public async Task<ChatbotResponse> ChatbotAsync(string mensagem)
    {
        if (string.IsNullOrWhiteSpace(mensagem))
        {
            throw new ArgumentException("A mensagem não pode ser vazia.");
        }

        var rawJson = await EnviarRequisicao(mensagem, SystemPromptChatbot);

        // Limpa o JSON do markdown se houver
        var inicio = rawJson.IndexOf('{');
        var fim = rawJson.LastIndexOf('}');
        var jsonLimpo = rawJson.Substring(inicio, fim - inicio + 1);

        var resultado = JsonSerializer.Deserialize<ChatbotResponse>(jsonLimpo, _jsonOptions);
        return resultado ?? new ChatbotResponse();
    }

    private async Task<string> EnviarRequisicao(string userMessage, string systemMessage)
    {
        var payload = new
        {
            model = "llama-3.3-70b-versatile",
            messages = new[]
            {
                new { role = "system", content = systemMessage },
                new { role = "user", content = userMessage }
            },
            temperature = 0.7,
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        
        var response = await _httpClient.PostAsync(ApiUrl, content);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        
        var groqResponse = JsonSerializer.Deserialize<GroqApiResponse>(responseJson, _jsonOptions);
        
        if (groqResponse != null && groqResponse.Choices != null && groqResponse.Choices.Count > 0)
        {
            return groqResponse.Choices[0].Message.Content;
        }

        throw new InvalidOperationException("A IA retornou uma resposta vazia.");
    }
}
