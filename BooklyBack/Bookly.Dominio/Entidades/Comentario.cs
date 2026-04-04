namespace Bookly.Dominio.Entidades;

public class Comentario
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid AvaliacaoId { get; set; }
    public string Texto { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    // Navegação
    public Usuario? Usuario { get; set; }
    public Avaliacao? Avaliacao { get; set; }
}
