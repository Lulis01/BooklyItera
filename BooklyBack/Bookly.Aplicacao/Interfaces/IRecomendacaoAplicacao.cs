using Bookly.Services.DTOs;

namespace Bookly.Aplicacao.Interfaces;

public interface IRecomendacaoAplicacao
{
    Task<IEnumerable<LivroRecomendadoDto>> GerarRecomendacoesAsync(IEnumerable<LivroAvaliadoDto> avaliacoes);
    Task<ChatbotResponse> EnviarMensagemChatbotAsync(string mensagem);
}
