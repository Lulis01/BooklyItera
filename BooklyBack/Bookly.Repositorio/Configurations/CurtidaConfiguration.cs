using Bookly.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Repositorio.Configurations;

public class CurtidaConfiguration : IEntityTypeConfiguration<Curtida>
{
    public void Configure(EntityTypeBuilder<Curtida> entity)
    {
        entity.ToTable("Curtidas");

        entity.HasKey(c => c.Id);

        entity.Property(c => c.DataCriacao)
              .IsRequired();

        // Relacionamento: Avaliacao (1) → Curtidas (N)
        entity.HasOne(c => c.Avaliacao)
              .WithMany(a => a.Curtidas)
              .HasForeignKey(c => c.AvaliacaoId)
              .OnDelete(DeleteBehavior.Cascade);

        // Relacionamento: Usuario (1) → Curtidas (N)
        entity.HasOne(c => c.Usuario)
              .WithMany(u => u.Curtidas)
              .HasForeignKey(c => c.UsuarioId)
              .OnDelete(DeleteBehavior.Restrict);

        // Um usuário só pode curtir uma avaliação uma vez
        entity.HasIndex(c => new { c.UsuarioId, c.AvaliacaoId })
              .IsUnique();
    }
}
