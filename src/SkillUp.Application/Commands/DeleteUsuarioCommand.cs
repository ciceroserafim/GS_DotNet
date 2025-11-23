using System;

namespace SkillUp.Application.Commands
{
    public class DeleteUsuarioCommand
    {
        public Guid Id { get; set; }

        public DeleteUsuarioCommand(Guid id)
        {
            Id = id;
        }
    }
}
