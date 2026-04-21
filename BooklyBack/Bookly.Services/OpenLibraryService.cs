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
            var isbn = doc.Isbn != null && doc.Isbn.Any() ? doc.Isbn.First() : string.Empty;
            // ISBN no banco é NVARCHAR(20) — truncar caso venha maior da API externa
            if (isbn.Length > 20) isbn = isbn.Substring(0, 20);

            var autor = doc.AuthorName != null && doc.AuthorName.Any()
                ? string.Join(", ", doc.AuthorName).Substring(0, Math.Min(string.Join(", ", doc.AuthorName).Length, 200))
                : "Desconhecido";

            var genero = doc.Subject != null && doc.Subject.Any()
                ? doc.Subject.First().Substring(0, Math.Min(doc.Subject.First().Length, 100))
                : string.Empty;

            livros.Add(new Livro
            {
                Id = Guid.NewGuid(),
                Titulo = doc.Title?.Substring(0, Math.Min(doc.Title.Length, 300)) ?? "Sem título",
                Autor = autor,
                AnoPublicacao = doc.FirstPublishYear ?? 0,
                ISBN = isbn,
                Genero = genero,
                DataCriacao = DateTime.UtcNow
            });
        }

        return livros;
    }
}
