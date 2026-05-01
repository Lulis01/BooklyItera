using Bookly.Services.DTOs;

namespace Bookly.Services.Interfaces;

public interface IGroqService
{
    Task<IEnumerable<LivroRecomendadoDto>> RecomendarLivrosAsync(IEnumerable<LivroAvaliadoDto> livrosAvaliados);

    Task<ChatbotResponse> ChatbotAsync(string mensagem);
}
