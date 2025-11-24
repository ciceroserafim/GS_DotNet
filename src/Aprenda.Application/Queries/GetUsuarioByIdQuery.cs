using System;

namespace Aprenda.Application.Queries
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
