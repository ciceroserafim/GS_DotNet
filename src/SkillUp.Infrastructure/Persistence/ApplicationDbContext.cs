using Microsoft.EntityFrameworkCore;
using SkillUp.Domain.Entities; // Ajuste o namespace das suas entidades

namespace SkillUp.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Aula> Aulas { get; set; }
        public DbSet<Modulo> Modulos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações adicionais (constraints, relacionamentos, etc)
            // Exemplo:
            // modelBuilder.Entity<Usuario>()
            //     .HasIndex(u => u.Email)
            //     .IsUnique();
        }
    }
}
