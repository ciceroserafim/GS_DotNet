using Microsoft.EntityFrameworkCore;
using SkillUp.Domain.Entities;

namespace SkillUp.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Modulo> Modulos { get; set; }
        public DbSet<Aula> Aulas { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Carrega automaticamente todas as IEntityTypeConfiguration<>
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
