namespace Bookly.Dominio.Entidades;

public class Curtida
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid AvaliacaoId { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    // Navegação
    public Usuario? Usuario { get; set; }
    public Avaliacao? Avaliacao { get; set; }
}
