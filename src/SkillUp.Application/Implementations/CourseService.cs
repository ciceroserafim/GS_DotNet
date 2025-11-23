using SkillUp.Application.DTOs;
using SkillUp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillUp.Application.Implementations
{
    public class CourseService : ICourseService
    {
        private static readonly List<CourseDto> _cursos = new();

        public Task<IEnumerable<CourseDto>> ListarAsync()
            => Task.FromResult<IEnumerable<CourseDto>>(_cursos);

        public Task<CourseDto?> ObterPorIdAsync(Guid id)
            => Task.FromResult(_cursos.FirstOrDefault(c => c.Id == id));

        public Task<CourseDto> CriarAsync(CourseDto dto)
        {
            dto.Id = Guid.NewGuid();
            _cursos.Add(dto);
            return Task.FromResult(dto);
        }

        public Task<bool> AtualizarAsync(Guid id, CourseDto dto)
        {
            var course = _cursos.FirstOrDefault(c => c.Id == id);
            if (course == null) return Task.FromResult(false);

            course.Nome = dto.Nome;
            course.Descricao = dto.Descricao;

            return Task.FromResult(true);
        }

        public Task<bool> DeletarAsync(Guid id)
        {
            var course = _cursos.FirstOrDefault(c => c.Id == id);
            if (course == null) return Task.FromResult(false);

            _cursos.Remove(course);
            return Task.FromResult(true);
        }
    }
}
