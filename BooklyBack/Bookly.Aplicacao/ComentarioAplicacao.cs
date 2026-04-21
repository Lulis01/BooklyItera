using Bookly.Aplicacao.Interfaces;
using Bookly.Dominio.Entidades;
using Bookly.Dominio.Interfaces;

namespace Bookly.Aplicacao;

public class ComentarioAplicacao : IComentarioAplicacao
{
    private readonly IComentarioRepositorio _comentarioRepositorio;

    public ComentarioAplicacao(IComentarioRepositorio comentarioRepositorio)
    {
        _comentarioRepositorio = comentarioRepositorio;
    }

    public async Task<int> CriarAsync(Comentario comentario)
    {
        if (comentario == null)
            throw new ArgumentNullException(nameof(comentario), "Comentário não pode ser vazio");

        if (comentario.UsuarioId == Guid.Empty)
            throw new Exception("Usuário do comentário não pode ser vazio");

        if (comentario.AvaliacaoId == Guid.Empty)
            throw new Exception("Avaliação do comentário não pode ser vazia");

        if (string.IsNullOrEmpty(comentario.Texto))
            throw new Exception("O texto do comentário não pode ser vazio.");

        return await _comentarioRepositorio.CriarAsync(comentario);
    }

    public async Task AtualizarAsync(Comentario comentario)
    {
        var comentarioExistente = await _comentarioRepositorio.ObterPorIdAsync(comentario.Id);
        if (comentarioExistente == null)
            throw new Exception("Comentário não encontrado");

        if (string.IsNullOrEmpty(comentario.Texto))
            throw new Exception("O texto do comentário não pode ser vazio.");

        comentarioExistente.Texto = comentario.Texto;

        await _comentarioRepositorio.AtualizarAsync(comentarioExistente);
    }

    public async Task DeletarAsync(Guid id)
    {
        var comentarioExistente = await _comentarioRepositorio.ObterPorIdAsync(id);
        if (comentarioExistente == null)
            throw new Exception("Comentário não encontrado");

        await _comentarioRepositorio.DeletarAsync(id);
    }

    public async Task<Comentario> ObterPorIdAsync(Guid id)
    {
        var comentario = await _comentarioRepositorio.ObterPorIdAsync(id);
        if (comentario == null)
            throw new Exception("Comentário não encontrado");

        return comentario;
    }

    public async Task<IEnumerable<Comentario>> ListarAsync()
    {
        return await _comentarioRepositorio.ListarAsync();
    }
}
