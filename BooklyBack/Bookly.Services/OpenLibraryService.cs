using System.Text.Json;
using Bookly.Dominio.Entidades;
using Bookly.Services.DTOs;
using Bookly.Services.Interfaces;

namespace Bookly.Services;

public class OpenLibraryService : IOpenLibraryService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://openlibrary.org";

    public OpenLibraryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<IEnumerable<Livro>> BuscarLivrosPorTituloAsync(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            return Enumerable.Empty<Livro>();

        // Usar o parâmetro 'title' foca estritamente em propriedades de livros/obras 
        var url = $"/search.json?title={Uri.EscapeDataString(titulo)}&limit=20";
        var response = await _httpClient.GetAsync(url);
        
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        
        var searchResult = JsonSerializer.Deserialize<OpenLibrarySearchResponse>(content, options);

        if (searchResult == null || searchResult.Docs == null || !searchResult.Docs.Any())
            return Enumerable.Empty<Livro>();

        // Mapeamento do JSON do OpenLibrary para a entidade Livro local
        var livros = new List<Livro>();
        foreach (var doc in searchResult.Docs)
        {
            livros.Add(new Livro
            {
                Id = Guid.NewGuid(), // ID temporário para identificação no front-end
                Titulo = doc.Title,
                Autor = doc.AuthorName != null && doc.AuthorName.Any() ? string.Join(", ", doc.AuthorName) : "Desconhecido",
                AnoPublicacao = doc.FirstPublishYear ?? 0,
                ISBN = doc.Isbn != null && doc.Isbn.Any() ? doc.Isbn.First() : string.Empty,
                Genero = doc.Subject != null && doc.Subject.Any() ? doc.Subject.First() : string.Empty,
                DataCriacao = DateTime.UtcNow
            });
        }

        return livros;
    }
}
