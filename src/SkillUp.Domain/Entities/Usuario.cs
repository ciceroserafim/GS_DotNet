namespace SkillUp.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }

        public Usuario(Guid id, string nome, string email, string senha)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Nome = nome;
            Email = email;
            Senha = senha;
        }

        public void Atualizar(string nome, string email, string senha)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
        }
    } 
} 
