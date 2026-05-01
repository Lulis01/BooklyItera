namespace Bookly.API.Models.Curtida;

public class CurtidaResponse
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid AvaliacaoId { get; set; }
    public DateTime DataCriacao { get; set; }
}
