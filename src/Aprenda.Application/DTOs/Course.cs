
using System;

namespace Aprenda.Application.DTOs
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
    }
}
