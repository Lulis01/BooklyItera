using Bookly.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Repositorio.Configurations;

public class ComentarioConfiguration : IEntityTypeConfiguration<Comentario>
{
    public void Configure(EntityTypeBuilder<Comentario> entity)
    {
        entity.ToTable("Comentarios");

        entity.HasKey(c => c.Id);

        entity.Property(c => c.Texto)
              .IsRequired()
              .HasMaxLength(600);

        entity.Property(c => c.DataCriacao)
              .IsRequired();

        // Relacionamento: Avaliacao (1) → Comentarios (N)
        entity.HasOne(c => c.Avaliacao)
              .WithMany(a => a.Comentarios)
              .HasForeignKey(c => c.AvaliacaoId)
              .OnDelete(DeleteBehavior.Cascade);

        // Relacionamento: Usuario (1) → Comentarios (N)
        entity.HasOne(c => c.Usuario)
              .WithMany(u => u.Comentarios)
              .HasForeignKey(c => c.UsuarioId)
              .OnDelete(DeleteBehavior.Restrict);
    }
}
