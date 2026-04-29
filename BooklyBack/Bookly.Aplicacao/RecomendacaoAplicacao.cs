using Bookly.Aplicacao.Interfaces;
using Bookly.Services.DTOs;
using Bookly.Services.Interfaces;

namespace Bookly.Aplicacao;

public class RecomendacaoAplicacao : IRecomendacaoAplicacao
{
    private readonly IGroqService _groqService;

    public RecomendacaoAplicacao(IGroqService groqService)
    {
        _groqService = groqService;
    }

    public async Task<IEnumerable<LivroRecomendadoDto>> GerarRecomendacoesAsync(
        IEnumerable<LivroAvaliadoDto> avaliacoes
    )
    {
        return await _groqService.RecomendarLivrosAsync(avaliacoes);
    }

    public async Task<ChatbotResponse> EnviarMensagemChatbotAsync(string mensagem)
    {
        return await _groqService.ChatbotAsync(mensagem);
    }
}
