using SkillUp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillUp.Application.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> ListarAsync();
        Task<CourseDto?> ObterPorIdAsync(Guid id);
        Task<CourseDto> CriarAsync(CourseDto dto);
        Task<bool> AtualizarAsync(Guid id, CourseDto dto);
        Task<bool> DeletarAsync(Guid id);
    }
}
