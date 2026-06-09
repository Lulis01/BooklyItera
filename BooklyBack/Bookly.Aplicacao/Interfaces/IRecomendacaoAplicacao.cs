using Bookly.Services.DTOs;

namespace Bookly.Aplicacao.Interfaces;

public interface IRecomendacaoAplicacao
{
    Task<IEnumerable<LivroRecomendadoDto>> GerarRecomendacoesPorUsuarioAsync(Guid usuarioId);
    Task<ChatbotResponse> EnviarMensagemChatbotAsync(string mensagem);
}
