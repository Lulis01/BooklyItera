using Bookly.Dominio.Entidades;

namespace Bookly.Dominio.Interfaces;

public interface IComentarioRepositorio
{
    Task<int> CriarAsync(Comentario comentario);
    Task AtualizarAsync(Comentario comentario);
    Task DeletarAsync(Guid id);
    Task<Comentario> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Comentario>> ListarAsync();
}
