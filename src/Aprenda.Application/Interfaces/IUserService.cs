using Aprenda.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aprenda.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UsuarioDto>> ListarAsync();
        Task<UsuarioDto?> ObterPorIdAsync(Guid id);
        Task<UsuarioDto> CriarAsync(UsuarioDto dto);
        Task<bool> AtualizarAsync(Guid id, UsuarioDto dto);
        Task<bool> DeletarAsync(Guid id);
    }
}
