namespace Bookly.Dominio.Entidades;

public class Avaliacao
{
    private string _texto = string.Empty;
    private int _nota;

    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid LivroId { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public string Texto
    {
        get => _texto;
        set
        {
            if (value != null && value.Length > 400)
                throw new ArgumentException("O texto da avaliação não pode ter mais de 400 caracteres.");
            _texto = value ?? string.Empty;
        }
    }

    public int Nota
    {
        get => _nota;
        set
        {
            if (value < 1 || value > 5)
                throw new ArgumentException("A nota deve ser entre 1 e 5.");
            _nota = value;
        }
    }

    // Navegação
    public Usuario? Usuario { get; set; }
    public Livro? Livro { get; set; }
    public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    public ICollection<Curtida> Curtidas { get; set; } = new List<Curtida>();
}
