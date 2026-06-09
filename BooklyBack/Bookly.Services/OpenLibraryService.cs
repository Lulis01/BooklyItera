using System.Text.Json;
using Bookly.Dominio.Entidades;
using Bookly.Services.DTOs;
using Bookly.Services.Interfaces;

namespace Bookly.Services;

public class OpenLibraryService : IOpenLibraryService
{
    private readonly HttpClient _httpClient;

    public OpenLibraryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://openlibrary.org");
    }

    public async Task<IEnumerable<Livro>> BuscarLivrosPorTituloAsync(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            return new List<Livro>();

        var url = "/search.json?title=" + Uri.EscapeDataString(titulo) + "&limit=15";
        var response = await _httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var searchResult = JsonSerializer.Deserialize<OpenLibrarySearchResponse>(content, options);

        if (searchResult == null || searchResult.Docs == null || searchResult.Docs.Count == 0)
            return new List<Livro>();

        var livros = new List<Livro>();

        foreach (var doc in searchResult.Docs)
        {
            
            string isbn = string.Empty;
            if (doc.Isbn != null && doc.Isbn.Count > 0)
                isbn = doc.Isbn[0];
            if (isbn.Length > 20)
                isbn = isbn.Substring(0, 20);

            
            string autor = "Desconhecido";
            if (doc.AuthorName != null && doc.AuthorName.Count > 0)
            {
                autor = string.Join(", ", doc.AuthorName);
                if (autor.Length > 200)
                    autor = autor.Substring(0, 200);
            }

            
            string genero = string.Empty;
            if (doc.Subject != null && doc.Subject.Count > 0)
            {
                genero = doc.Subject[0];
                if (genero.Length > 100)
                    genero = genero.Substring(0, 100);
            }

            
            string tituloLivro = "Sem título";
            if (doc.Title != null)
            {
                tituloLivro = doc.Title;
                if (tituloLivro.Length > 300)
                    tituloLivro = tituloLivro.Substring(0, 300);
            }

            var livro = new Livro
            {
                Id = Guid.NewGuid(),
                Titulo = tituloLivro,
                Autor = autor,
                AnoPublicacao = doc.FirstPublishYear ?? 0,
                ISBN = isbn,
                Genero = genero,
                DataCriacao = DateTime.UtcNow
            };

            livros.Add(livro);
        }

        return livros;
    }
}
