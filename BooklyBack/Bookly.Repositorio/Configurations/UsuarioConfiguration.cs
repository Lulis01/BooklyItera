using Bookly.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Repositorio.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> entity)
    {
        entity.ToTable("Usuarios");

        entity.HasKey(u => u.Id);

        entity.Property(u => u.Nome)
              .IsRequired()
              .HasMaxLength(150);

        entity.Property(u => u.Email)
              .IsRequired()
              .HasMaxLength(200);

        entity.HasIndex(u => u.Email)
              .IsUnique();

        entity.Property(u => u.SenhaHash)
              .IsRequired()
              .HasMaxLength(500);

        entity.Property(u => u.DataCriacao)
              .IsRequired();
    }
}
