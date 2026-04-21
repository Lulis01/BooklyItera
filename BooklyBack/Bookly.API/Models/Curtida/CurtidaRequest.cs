namespace Bookly.API.Models.Curtida;

public class CriarCurtidaRequest
{
    public Guid UsuarioId { get; set; }
    public Guid AvaliacaoId { get; set; }
}
