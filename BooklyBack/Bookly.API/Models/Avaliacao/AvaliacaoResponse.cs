namespace Bookly.API.Models.Avaliacao;

public class AvaliacaoResponse
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid LivroId { get; set; }
    public string Texto { get; set; } = string.Empty;
    public int Nota { get; set; }
    public DateTime DataCriacao { get; set; }
}
