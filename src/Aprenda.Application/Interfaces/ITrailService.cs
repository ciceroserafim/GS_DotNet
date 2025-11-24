using Aprenda.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aprenda.Application.Interfaces
{
    public interface ITrailService
    {
        Task<IEnumerable<TrailDto>> ListarAsync();
        Task<TrailDto?> ObterPorIdAsync(Guid id);
        Task<TrailDto> CriarAsync(TrailDto dto);
        Task<bool> AtualizarAsync(Guid id, TrailDto dto);
        Task<bool> DeletarAsync(Guid id);
    }
}
