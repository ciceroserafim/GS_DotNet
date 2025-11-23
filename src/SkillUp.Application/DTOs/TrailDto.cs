using System;

namespace SkillUp.Application.DTOs
{
    public class TrailDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
    }
}
