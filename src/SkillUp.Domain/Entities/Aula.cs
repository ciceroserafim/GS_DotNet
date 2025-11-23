namespace SkillUp.Domain.Entities
{
	public class Aula
	{
		public Guid Id { get; private set; }
		public string Titulo { get; private set; }
		public string Conteudo { get; private set; }
		public Guid ModuloId { get; private set; }

		public Aula(Guid id, string titulo, string conteudo, Guid moduloId)
		{
			Id = id == Guid.Empty ? Guid.NewGuid() : id;
			Titulo = titulo;
			Conteudo = conteudo;
			ModuloId = moduloId;
		}

		public void Atualizar(string titulo, string conteudo)
		{
			Titulo = titulo;
			Conteudo = conteudo;
		}
	}
}
