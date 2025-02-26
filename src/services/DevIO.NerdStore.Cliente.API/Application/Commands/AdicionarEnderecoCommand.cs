using DevIO.NerdStore.Core.Messages;
using FluentValidation;

namespace DevIO.NerdStore.Clientes.API.Application.Commands;

public class AdicionarEnderecoCommand(
    Guid clienteId,
    string logradouro,
    string numero,
    string complemento,
    string bairro,
    string cep,
    string cidade,
    string estado) : Command
{
    public Guid ClienteId { get; set; } = clienteId;
    public string Logradouro { get; set; } = logradouro;
    public string Numero { get; set; } = numero;
    public string Complemento { get; set; } = complemento;
    public string Bairro { get; set; } = bairro;
    public string Cep { get; set; } = cep;
    public string Cidade { get; set; } = cidade;
    public string Estado { get; set; } = estado;

    public override bool EhValido()
    {
        ValidationResult = new EnderecoValidation().Validate(this);

        return ValidationResult.IsValid;
    }

    public class EnderecoValidation : AbstractValidator<AdicionarEnderecoCommand>
    {
        public EnderecoValidation()
        {
            RuleFor(c => c.Logradouro)
                .NotEmpty()
                .WithMessage("Informe o Logradouro");

            RuleFor(c => c.Numero)
                .NotEmpty()
                .WithMessage("Informe o Número");

            RuleFor(c => c.Cep)
                .NotEmpty()
                .WithMessage("Informe o CEP");

            RuleFor(c => c.Bairro)
                .NotEmpty()
                .WithMessage("Informe o Bairro");

            RuleFor(c => c.Cidade)
                .NotEmpty()
                .WithMessage("Informe o Cidade");

            RuleFor(c => c.Estado)
                .NotEmpty()
                .WithMessage("Informe o Estado");
        }
    }
}
