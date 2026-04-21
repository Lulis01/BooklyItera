namespace Bookly.API.Models.Livro;

public class LivroResponse
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int AnoPublicacao { get; set; }
    public string Genero { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
}
