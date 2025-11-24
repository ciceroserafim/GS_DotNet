using Aprenda.Domain.Entities;

namespace Aprenda.Domain.Interfaces
{
    public interface ICursoRepository
    {
        Task<Curso?> GetByIdAsync(Guid id);
        Task<IEnumerable<Curso>> ListAsync();
        Task AddAsync(Curso curso);
        Task UpdateAsync(Curso curso);
        Task DeleteAsync(Guid id);
    }
}
