using Bookly.Dominio.Entidades;

namespace Bookly.Aplicacao.Interfaces;

public interface IUsuarioAplicacao
{
    Task<int> CriarAsync(Usuario usuario);
    Task AtualizarAsync(Usuario usuario);
    Task DeletarAsync(Guid id);
    Task<Usuario> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Usuario>> ListarAsync();
}
