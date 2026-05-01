using Bookly.Dominio.Entidades;

namespace Bookly.Aplicacao.Interfaces;

public interface ICurtidaAplicacao
{
    Task<int> CriarAsync(Curtida curtida);
    Task DeletarAsync(Guid id);
    Task<Curtida> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Curtida>> ListarAsync();
}
