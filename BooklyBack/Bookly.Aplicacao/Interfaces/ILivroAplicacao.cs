using Bookly.Dominio.Entidades;

namespace Bookly.Aplicacao.Interfaces;

public interface ILivroAplicacao
{
    Task<int> CriarAsync(Livro livro);
    Task AtualizarAsync(Livro livro);
    Task DeletarAsync(Guid id);
    Task<Livro> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Livro>> ListarAsync();
}
