using Aprenda.Application.DTOs;
using System;

namespace Aprenda.Application.Commands
{
    public class UpdateUsuarioCommand
    {
        public Guid Id { get; set; }
        public UsuarioDto Usuario { get; set; }

        public UpdateUsuarioCommand(Guid id, UsuarioDto usuario)
        {
            Id = id;
            Usuario = usuario;
        }
    }
}
