using SkillUp.Application.DTOs;
using SkillUp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillUp.Application.Implementations
{
    public class TrailService : ITrailService
    {
        private static readonly List<TrailDto> _trails = new();

        public Task<IEnumerable<TrailDto>> ListarAsync()
            => Task.FromResult<IEnumerable<TrailDto>>(_trails);

        public Task<TrailDto?> ObterPorIdAsync(Guid id)
            => Task.FromResult(_trails.FirstOrDefault(t => t.Id == id));

        public Task<TrailDto> CriarAsync(TrailDto dto)
        {
            dto.Id = Guid.NewGuid();
            _trails.Add(dto);
            return Task.FromResult(dto);
        }

        public Task<bool> AtualizarAsync(Guid id, TrailDto dto)
        {
            var trail = _trails.FirstOrDefault(t => t.Id == id);
            if (trail == null) return Task.FromResult(false);

            trail.Nome = dto.Nome;
            trail.Descricao = dto.Descricao;

            return Task.FromResult(true);
        }

        public Task<bool> DeletarAsync(Guid id)
        {
            var trail = _trails.FirstOrDefault(t => t.Id == id);
            if (trail == null) return Task.FromResult(false);

            _trails.Remove(trail);
            return Task.FromResult(true);
        }
    }
}
