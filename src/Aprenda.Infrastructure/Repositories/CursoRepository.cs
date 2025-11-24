using Microsoft.EntityFrameworkCore;
using Aprenda.Domain.Entities;
using Aprenda.Domain.Interfaces;
using Aprenda.Infrastructure.Persistence;

namespace Aprenda.Infrastructure.Repositories
{
    public class CursoRepository : ICursoRepository
    {
        private readonly AppDbContext _context;

        public CursoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Curso?> GetByIdAsync(Guid id)
        {
            return await _context.Cursos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Curso>> ListAsync()
        {
            return await _context.Cursos.ToListAsync();
        }

        public async Task AddAsync(Curso curso)
        {
            await _context.Cursos.AddAsync(curso);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Curso curso)
        {
            _context.Cursos.Update(curso);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var curso = await GetByIdAsync(id);
            if (curso == null) return;

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();
        }
    }
}
