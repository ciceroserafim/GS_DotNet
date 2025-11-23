using SkillUp.Domain.Exceptions;

namespace SkillUp.Domain.ValueObjects
{
	public class NomeCompleto
	{
		public string PrimeiroNome { get; private set; }
		public string Sobrenome { get; private set; }

		public NomeCompleto(string primeiroNome, string sobrenome)
		{
			if (string.IsNullOrWhiteSpace(primeiroNome))
				throw new DomainException("Primeiro nome inválido.");

			if (string.IsNullOrWhiteSpace(sobrenome))
				throw new DomainException("Sobrenome inválido.");

			PrimeiroNome = primeiroNome;
			Sobrenome = sobrenome;
		}

		public override string ToString() => $"{PrimeiroNome} {Sobrenome}";
	}
}
