using System;

namespace Aprenda.Domain.Entities
{
    public class Matricula
    {
        public int Id { get; set; }
        public DateTime DataMatricula { get; set; } = DateTime.UtcNow;
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
