using Bookly.Dominio.Entidades;

namespace Bookly.Dominio.Interfaces;

public interface ICurtidaRepositorio
{
    Task<int> CriarAsync(Curtida curtida);
    Task AtualizarAsync(Curtida curtida);
    Task DeletarAsync(Guid id);
    Task<Curtida> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Curtida>> ListarAsync();
}
