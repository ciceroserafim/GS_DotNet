namespace SkillUp.Domain.Entities
{
    public class Matricula
    {
        public int Id { get; set; }
        public DateTime DataMatricula { get; set; } = DateTime.UtcNow;

     
        public Usuario Usuario { get; set; }
        public int UsuarioId { get; set; }

    }
}
