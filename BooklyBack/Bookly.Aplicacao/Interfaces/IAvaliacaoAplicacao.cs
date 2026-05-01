using Bookly.Dominio.Entidades;

namespace Bookly.Aplicacao.Interfaces;

public interface IAvaliacaoAplicacao
{
    Task<int> CriarAsync(Avaliacao avaliacao);
    Task AtualizarAsync(Avaliacao avaliacao);
    Task DeletarAsync(Guid id);
    Task<Avaliacao> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Avaliacao>> ListarAsync();
}
