using Bookly.Aplicacao.Interfaces;
using Bookly.Dominio.Entidades;
using Bookly.Dominio.Interfaces;
using Bookly.Services.Interfaces;

namespace Bookly.Aplicacao;

public class LivroAplicacao : ILivroAplicacao
{
    private readonly ILivroRepositorio _livroRepositorio;
    private readonly IOpenLibraryService _openLibraryService;

    public LivroAplicacao(ILivroRepositorio livroRepositorio, IOpenLibraryService openLibraryService)
    {
        _livroRepositorio = livroRepositorio;
        _openLibraryService = openLibraryService;
    }

    public async Task<int> CriarAsync(Livro livro)
    {
        if (livro == null)
            throw new ArgumentNullException(nameof(livro), "Livro não pode ser vazio");

        if (string.IsNullOrEmpty(livro.Titulo))
            throw new Exception("Título não pode ser vazio");

        if (string.IsNullOrEmpty(livro.Autor))
            throw new Exception("Autor não pode ser vazio");


        if (livro.ISBN == null)
            livro.ISBN = string.Empty;

        return await _livroRepositorio.CriarAsync(livro);
    }

    public async Task AtualizarAsync(Livro livro)
    {
        var livroExistente = await _livroRepositorio.ObterPorIdAsync(livro.Id);
        if (livroExistente == null)
            throw new Exception("Livro não encontrado");

        if (string.IsNullOrEmpty(livro.Titulo))
            throw new Exception("Título não pode ser vazio");

        livroExistente.Titulo = livro.Titulo;
        livroExistente.Autor = livro.Autor;
        livroExistente.ISBN = livro.ISBN;
        livroExistente.AnoPublicacao = livro.AnoPublicacao;
        livroExistente.Genero = livro.Genero;

        await _livroRepositorio.AtualizarAsync(livroExistente);
    }

    public async Task DeletarAsync(Guid id)
    {
        var livroExistente = await _livroRepositorio.ObterPorIdAsync(id);
        if (livroExistente == null)
            throw new Exception("Livro não encontrado");

        await _livroRepositorio.DeletarAsync(id);
    }

    public async Task<Livro> ObterPorIdAsync(Guid id)
    {
        var livro = await _livroRepositorio.ObterPorIdAsync(id);
        if (livro == null)
            throw new Exception("Livro não encontrado");

        return livro;
    }

    public async Task<IEnumerable<Livro>> ListarAsync()
    {
        return await _livroRepositorio.ListarAsync();
    }

    public async Task<IEnumerable<Livro>> BuscarLivrosExternosAsync(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            return Enumerable.Empty<Livro>();
            
        return await _openLibraryService.BuscarLivrosPorTituloAsync(titulo);
    }
}
