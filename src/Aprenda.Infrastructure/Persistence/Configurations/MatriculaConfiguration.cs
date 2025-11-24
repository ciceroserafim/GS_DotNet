using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Aprenda.Domain.Entities;

namespace Aprenda.Infrastructure.Persistence.Configurations
{
    public class MatriculaConfiguration : IEntityTypeConfiguration<Matricula>
    {
        public void Configure(EntityTypeBuilder<Matricula> builder)
        {
            builder.ToTable("Matriculas");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.DataMatricula)
                .IsRequired();

            builder.HasOne(m => m.Usuario)
                .WithMany()
                .HasForeignKey(m => m.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

