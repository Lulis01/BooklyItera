using Bookly.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Bookly.Repositorio;

public class BooklyDbContext : DbContext
{
    public BooklyDbContext()
    {
    }

    public BooklyDbContext(DbContextOptions<BooklyDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=Lulis;Database=BooklyDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Livro> Livros { get; set; }
    public DbSet<Avaliacao> Avaliacoes { get; set; }
    public DbSet<Comentario> Comentarios { get; set; }
    public DbSet<Curtida> Curtidas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica automaticamente todas as classes IEntityTypeConfiguration<T>
        // presentes no assembly (pasta Configurations)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BooklyDbContext).Assembly);
    }
}
