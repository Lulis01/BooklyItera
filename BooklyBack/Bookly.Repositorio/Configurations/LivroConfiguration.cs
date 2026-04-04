using Bookly.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Repositorio.Configurations;

public class LivroConfiguration : IEntityTypeConfiguration<Livro>
{
    public void Configure(EntityTypeBuilder<Livro> entity)
    {
        entity.ToTable("Livros");

        entity.HasKey(l => l.Id);

        entity.Property(l => l.Titulo)
              .IsRequired()
              .HasMaxLength(300);

        entity.Property(l => l.Autor)
              .IsRequired()
              .HasMaxLength(200);

        entity.Property(l => l.ISBN)
              .HasMaxLength(20);

        entity.Property(l => l.Genero)
              .HasMaxLength(100);

        entity.Property(l => l.DataCriacao)
              .IsRequired();
    }
}
