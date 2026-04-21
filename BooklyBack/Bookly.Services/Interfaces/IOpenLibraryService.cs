using Bookly.Dominio.Entidades;

namespace Bookly.Services.Interfaces;

public interface IOpenLibraryService
{
    Task<IEnumerable<Livro>> BuscarLivrosPorTituloAsync(string titulo);
}
