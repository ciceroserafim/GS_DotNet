using Aprenda.Application.DTOs;

namespace Aprenda.Application.Commands
{
    public class CreateUsuarioCommand
    {
        public UsuarioDto Usuario { get; set; }

        public CreateUsuarioCommand(UsuarioDto usuario)
        {
            Usuario = usuario;
        }
    }
}
