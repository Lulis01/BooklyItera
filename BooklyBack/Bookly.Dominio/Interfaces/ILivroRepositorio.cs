using Bookly.Dominio.Entidades;

namespace Bookly.Dominio.Interfaces;

public interface ILivroRepositorio
{
    Task<int> CriarAsync(Livro livro);
    Task AtualizarAsync(Livro livro);
    Task DeletarAsync(Guid id);
    Task<Livro> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Livro>> ListarAsync();
}
