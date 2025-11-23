using System;

namespace SkillUp.Application.Queries
{
    public class GetUsuarioByIdQuery
    {
        public Guid Id { get; set; }

        public GetUsuarioByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
