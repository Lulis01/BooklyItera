namespace Bookly.API.Models.Comentario;

public class CriarComentarioRequest
{
    public Guid UsuarioId { get; set; }
    public Guid AvaliacaoId { get; set; }
    public string Texto { get; set; } = string.Empty;
}

public class AtualizarComentarioRequest
{
    public string Texto { get; set; } = string.Empty;
}
