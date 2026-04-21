using Bookly.Dominio.Entidades;

namespace Bookly.Aplicacao.Interfaces;

public interface IComentarioAplicacao
{
    Task<int> CriarAsync(Comentario comentario);
    Task AtualizarAsync(Comentario comentario);
    Task DeletarAsync(Guid id);
    Task<Comentario> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Comentario>> ListarAsync();
}
