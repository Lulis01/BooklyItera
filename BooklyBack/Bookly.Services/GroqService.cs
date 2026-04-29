using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Bookly.Services.DTOs;
using Bookly.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Bookly.Services;

public class GroqService : IGroqService
{
    private readonly HttpClient          _httpClient;
    private readonly string              _apiKey;
    private readonly JsonSerializerOptions _jsonOptions;

    private const string ApiUrl     = "https://api.groq.com/openai/v1/chat/completions";
    private const string Model      = "llama-3.3-70b-versatile";
    private const int    NotaMinima = 3;

    private const string SystemPromptChatbot = """
        Você é o Bookly Bot, um especialista apaixonado por literatura que adora ajudar pessoas
        a descobrirem o próximo livro favorito. Você conhece milhares de livros de todos os gêneros,
        épocas e culturas. Seu tom é caloroso, entusiasmado e amigável.

        Regras obrigatórias:
        - Sempre retorne entre 5 e 10 livros reais (nunca invente títulos).
        - Nunca repita livros que o usuário já mencionou ter lido.
        - Adapte as recomendações ao pedido: gênero, emoção, ritmo, complexidade, faixa etária.
        - Se o pedido for vago, sugira clássicos variados e populares.
        - Responda SOMENTE com um JSON válido, sem markdown, sem explicações fora do JSON.

        Formato de saída (JSON puro):
        {
          "mensagem": "Texto amigável e entusiasmado explicando as sugestões (2-3 frases)",
          "recomendacoes": [
            {
              "titulo": "Nome do Livro",
              "autor": "Nome do Autor",
              "motivo": "Motivo curto e empolgante da recomendação"
            }
          ]
        }
        """;

    public GroqService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey     = configuration["Groq:ApiKey"]
            ?? throw new InvalidOperationException("Chave da API Groq não configurada (Groq:ApiKey).");

        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<LivroRecomendadoDto>> RecomendarLivrosAsync(
        IEnumerable<LivroAvaliadoDto> livrosAvaliados)
    {
        var livrosFiltrados = livrosAvaliados
            .Where(l => l.Nota >= NotaMinima)
            .ToList();

        var prompt = MontarPromptRecomendacao(livrosFiltrados);
        var rawJson = await ChamarGroqAsync(prompt, systemPrompt: null);

        return ExtrairArray(rawJson);
    }

    public async Task<ChatbotResponse> ChatbotAsync(string mensagem)
    {
        if (string.IsNullOrWhiteSpace(mensagem))
            throw new ArgumentException("A mensagem não pode ser vazia.", nameof(mensagem));

        var rawJson = await ChamarGroqAsync(
            userMessage: mensagem,
            systemPrompt: SystemPromptChatbot);

        return ExtrairChatbotResponse(rawJson);
    }

    private static string MontarPromptRecomendacao(List<LivroAvaliadoDto> livros)
    {
        var lista = livros.Any()
            ? string.Join("\n", livros.Select(l => $"- \"{l.Titulo}\" de {l.Autor} (nota: {l.Nota}/5)"))
            : "Nenhum livro avaliado ainda.";

        return $$"""
            Você é um especialista em literatura e recomendação de livros.

            O usuário avaliou positivamente os seguintes livros:
            {{lista}}

            Com base nesses gostos, recomende exatamente 10 livros que o usuário ainda não leu.

            Regras:
            - NÃO repita nenhum dos livros já listados acima.
            - Priorize semelhanças em: gênero, estilo, temas, público-alvo e tom narrativo.
            - Se houver poucos dados, complete com clássicos populares relacionados.
            - Retorne APENAS um array JSON válido, sem texto adicional, sem markdown.

            Formato de saída (JSON puro):
            [
              {
                "titulo": "Nome do Livro",
                "autor": "Nome do Autor",
                "motivo": "Breve explicação da recomendação"
              }
            ]
            """;
    }

    private async Task<string> ChamarGroqAsync(string userMessage, string? systemPrompt)
    {
        var messages = new List<object>();

        if (!string.IsNullOrEmpty(systemPrompt))
            messages.Add(new { role = "system", content = systemPrompt });

        messages.Add(new { role = "user", content = userMessage });

        var payload = new
        {
            model       = Model,
            messages,
            temperature = 0.7
        };

        var json    = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _apiKey);

        var response = await _httpClient.PostAsync(ApiUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            var erro = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(
                $"Erro na API Groq. Status: {response.StatusCode}. Detalhe: {erro}");
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        var groqResponse = JsonSerializer.Deserialize<GroqApiResponse>(responseJson, _jsonOptions);

        return groqResponse?.Choices?.FirstOrDefault()?.Message?.Content
            ?? throw new InvalidOperationException("A IA retornou uma resposta vazia.");
    }

    private IEnumerable<LivroRecomendadoDto> ExtrairArray(string rawContent)
    {
        var inicio = rawContent.IndexOf('[');
        var fim    = rawContent.LastIndexOf(']');

        if (inicio < 0 || fim < 0)
            throw new InvalidOperationException("A IA não retornou um array JSON válido.");

        var jsonLimpo = rawContent[inicio..(fim + 1)];

        return JsonSerializer.Deserialize<List<LivroRecomendadoDto>>(jsonLimpo, _jsonOptions)
            ?? throw new InvalidOperationException("Não foi possível desserializar as recomendações.");
    }

    private ChatbotResponse ExtrairChatbotResponse(string rawContent)
    {
        var inicio = rawContent.IndexOf('{');
        var fim    = rawContent.LastIndexOf('}');

        if (inicio < 0 || fim < 0)
            throw new InvalidOperationException("A IA não retornou um objeto JSON válido.");

        var jsonLimpo = rawContent[inicio..(fim + 1)];

        return JsonSerializer.Deserialize<ChatbotResponse>(jsonLimpo, _jsonOptions)
            ?? throw new InvalidOperationException("Não foi possível desserializar a resposta do chatbot.");
    }
}
