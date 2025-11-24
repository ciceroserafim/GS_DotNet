namespace Aprenda.Domain.Entities
{
	public class Curso
	{
		public Guid Id { get; private set; }
		public string Titulo { get; private set; }
		public string Descricao { get; private set; }

		public Curso(Guid id, string titulo, string descricao)
		{
			Id = id == Guid.Empty ? Guid.NewGuid() : id;
			Titulo = titulo;
			Descricao = descricao;
		}

		public void Atualizar(string titulo, string descricao)
		{
			Titulo = titulo;
			Descricao = descricao;
		}
	}
}
