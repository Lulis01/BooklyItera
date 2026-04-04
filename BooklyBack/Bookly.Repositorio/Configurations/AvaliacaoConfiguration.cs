using Bookly.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Repositorio.Configurations;

public class AvaliacaoConfiguration : IEntityTypeConfiguration<Avaliacao>
{
    public void Configure(EntityTypeBuilder<Avaliacao> entity)
    {
        entity.ToTable("Avaliacoes");

        entity.HasKey(a => a.Id);

        entity.Property(a => a.Texto)
              .IsRequired()
              .HasMaxLength(400);

        entity.Property(a => a.Nota)
              .IsRequired();

        entity.Property(a => a.DataCriacao)
              .IsRequired();

        // Relacionamento: Usuario (1) → Avaliacoes (N)
        entity.HasOne(a => a.Usuario)
              .WithMany(u => u.Avaliacoes)
              .HasForeignKey(a => a.UsuarioId)
              .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento: Livro (1) → Avaliacoes (N)
        entity.HasOne(a => a.Livro)
              .WithMany(l => l.Avaliacoes)
              .HasForeignKey(a => a.LivroId)
              .OnDelete(DeleteBehavior.Restrict);
    }
}
