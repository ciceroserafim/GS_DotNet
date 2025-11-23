using Microsoft.EntityFrameworkCore;
using SkillUp.Domain.Entities;
using SkillUp.Domain.Interfaces;
using SkillUp.Infrastructure.Persistence;

namespace SkillUp.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByIdAsync(Guid id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Usuario>> ListAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task AddAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var usuario = await GetByIdAsync(id);
            if (usuario == null) return;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }
    }
}
