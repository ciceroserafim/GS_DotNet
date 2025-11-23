using SkillUp.Application.DTOs;

namespace SkillUp.Application.Commands
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
