using Aprenda.Application.DTOs;
using Aprenda.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aprenda.Application.Implementations
{
    public class UserService : IUserService
    {
        private static readonly List<UsuarioDto> _usuarios = new();

        public Task<IEnumerable<UsuarioDto>> ListarAsync()
            => Task.FromResult<IEnumerable<UsuarioDto>>(_usuarios);

        public Task<UsuarioDto?> ObterPorIdAsync(Guid id)
            => Task.FromResult(_usuarios.FirstOrDefault(u => u.Id == id));

        public Task<UsuarioDto> CriarAsync(UsuarioDto dto)
        {
            dto.Id = Guid.NewGuid();
            _usuarios.Add(dto);
            return Task.FromResult(dto);
        }

        public Task<bool> AtualizarAsync(Guid id, UsuarioDto dto)
        {
            var user = _usuarios.FirstOrDefault(u => u.Id == id);
            if (user == null) return Task.FromResult(false);

            user.Nome = dto.Nome;
            user.Email = dto.Email;
            user.Senha = dto.Senha;

            return Task.FromResult(true);
        }

        public Task<bool> DeletarAsync(Guid id)
        {
            var user = _usuarios.FirstOrDefault(u => u.Id == id);
            if (user == null) return Task.FromResult(false);

            _usuarios.Remove(user);
            return Task.FromResult(true);
        }
    }
}
