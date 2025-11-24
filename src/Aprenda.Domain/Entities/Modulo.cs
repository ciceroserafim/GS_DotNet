namespace Aprenda.Domain.Entities
{
	public class Modulo
	{
		public Guid Id { get; private set; }
		public string Nome { get; private set; }
		public Guid CursoId { get; private set; }

		public Modulo(Guid id, string nome, Guid cursoId)
		{
			Id = id == Guid.Empty ? Guid.NewGuid() : id;
			Nome = nome;
			CursoId = cursoId;
		}

		public void Atualizar(string nome)
		{
			Nome = nome;
		}
	}
}
