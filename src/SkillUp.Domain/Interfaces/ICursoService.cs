using SkillUp.Domain.Entities;

namespace SkillUp.Domain.Interfaces
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
