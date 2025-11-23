using SkillUp.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace SkillUp.Domain.ValueObjects
{
    public class Email
    {
        public string Endereco { get; private set; }

        public Email(string endereco)
        {
            if (string.IsNullOrWhiteSpace(endereco))
                throw new DomainException("E-mail não pode ser vazio.");

            if (!Regex.IsMatch(endereco, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new DomainException("E-mail inválido.");

            Endereco = endereco;
        }

        public override string ToString() => Endereco;
    }
}
