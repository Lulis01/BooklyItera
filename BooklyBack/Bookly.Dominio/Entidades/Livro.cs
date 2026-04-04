namespace Bookly.Dominio.Entidades;

public class Livro
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int AnoPublicacao { get; set; }
    public string Genero { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    // Navegação
    public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
}
