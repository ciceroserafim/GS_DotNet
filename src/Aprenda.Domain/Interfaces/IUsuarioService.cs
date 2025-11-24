using Aprenda.Domain.Entities;

namespace Aprenda.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByIdAsync(Guid id);
        Task<IEnumerable<Usuario>> ListAsync();
        Task AddAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task DeleteAsync(Guid id);
    }
}
