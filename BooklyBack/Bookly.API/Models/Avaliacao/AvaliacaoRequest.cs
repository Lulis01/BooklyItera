namespace Bookly.API.Models.Avaliacao;

public class CriarAvaliacaoRequest
{
    public Guid UsuarioId { get; set; }
    public Guid LivroId { get; set; }
    public string Texto { get; set; } = string.Empty;
    public int Nota { get; set; }
}

public class AtualizarAvaliacaoRequest
{
    public string Texto { get; set; } = string.Empty;
    public int Nota { get; set; }
}
