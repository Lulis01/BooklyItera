namespace Bookly.API.Models.Comentario;

public class ComentarioResponse
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid AvaliacaoId { get; set; }
    public string Texto { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
}
