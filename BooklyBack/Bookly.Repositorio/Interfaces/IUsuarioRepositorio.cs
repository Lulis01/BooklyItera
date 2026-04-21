using Bookly.Dominio.Entidades;

namespace Bookly.Dominio.Interfaces;

public interface IUsuarioRepositorio
{
    Task<int> CriarAsync(Usuario usuario);
    Task AtualizarAsync(Usuario usuario);
    Task DeletarAsync(Guid id);
    Task<Usuario> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Usuario>> ListarAsync();
}
