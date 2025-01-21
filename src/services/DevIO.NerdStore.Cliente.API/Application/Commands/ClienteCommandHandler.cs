using DevIO.NerdStore.Clientes.API.Models;
using DevIO.NerdStore.Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace DevIO.NerdStore.Clientes.API.Application.Commands;

public class ClienteCommandHandler : CommandHandler, IRequestHandler<RegistrarClienteCommand, ValidationResult>
{
    public async Task<ValidationResult?> Handle(RegistrarClienteCommand message, CancellationToken cancellationToken)
    {
        if (!message.EhValido())
            return message.ValidationResult;

        Cliente cliente = new(message.Id, message.Nome, message.Email, message.Cpf);

        return message.ValidationResult;
    }
}
