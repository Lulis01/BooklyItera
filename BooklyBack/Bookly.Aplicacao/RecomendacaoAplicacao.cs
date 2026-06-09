using Bookly.Aplicacao.Interfaces;
using Bookly.Dominio.Interfaces;
using Bookly.Services.DTOs;
using Bookly.Services.Interfaces;

namespace Bookly.Aplicacao;

public class RecomendacaoAplicacao : IRecomendacaoAplicacao
{
    private readonly IGroqService _groqService;
    private readonly IAvaliacaoRepositorio _avaliacaoRepositorio;
    private readonly ILivroRepositorio _livroRepositorio;

    public RecomendacaoAplicacao(
        IGroqService groqService,
        IAvaliacaoRepositorio avaliacaoRepositorio,
        ILivroRepositorio livroRepositorio
    )
    {
        _groqService = groqService;
        _avaliacaoRepositorio = avaliacaoRepositorio;
        _livroRepositorio = livroRepositorio;
    }

    public async Task<IEnumerable<LivroRecomendadoDto>> GerarRecomendacoesPorUsuarioAsync(Guid usuarioId)
    {
        if (usuarioId == Guid.Empty)
            throw new ArgumentException("O usuário informado é inválido.");

        var todasAvaliacoes = await _avaliacaoRepositorio.ListarAsync();
        var avaliacoesDoUsuario = todasAvaliacoes.Where(a => a.UsuarioId == usuarioId).ToList();

        if (avaliacoesDoUsuario.Count == 0)
            throw new InvalidOperationException("É necessário ter ao menos uma avaliação para gerar recomendações.");

        var livrosAvaliados = new List<LivroAvaliadoDto>();

        foreach (var avaliacao in avaliacoesDoUsuario)
        {
            var livro = await _livroRepositorio.ObterPorIdAsync(avaliacao.LivroId);
            if (livro == null)
                continue;

            livrosAvaliados.Add(new LivroAvaliadoDto
            {
                Titulo = livro.Titulo,
                Autor = livro.Autor,
                Nota = avaliacao.Nota
            });
        }

        if (livrosAvaliados.Count == 0)
            throw new InvalidOperationException("Não foi possível encontrar os livros das suas avaliações.");

        return await _groqService.RecomendarLivrosAsync(livrosAvaliados);
    }

    public async Task<ChatbotResponse> EnviarMensagemChatbotAsync(string mensagem)
    {
        return await _groqService.ChatbotAsync(mensagem);
    }
}
