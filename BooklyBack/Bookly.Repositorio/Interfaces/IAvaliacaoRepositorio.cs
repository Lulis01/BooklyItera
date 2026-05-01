using Bookly.Dominio.Entidades;

namespace Bookly.Dominio.Interfaces;

public interface IAvaliacaoRepositorio
{
    Task<int> CriarAsync(Avaliacao avaliacao);
    Task AtualizarAsync(Avaliacao avaliacao);
    Task DeletarAsync(Guid id);
    Task<Avaliacao> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Avaliacao>> ListarAsync();
}
