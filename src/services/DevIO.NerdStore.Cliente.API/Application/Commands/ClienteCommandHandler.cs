﻿using DevIO.NerdStore.Clientes.API.Application.Events;
using DevIO.NerdStore.Clientes.API.Models;
using DevIO.NerdStore.Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace DevIO.NerdStore.Clientes.API.Application.Commands;

public class ClienteCommandHandler(IClienteRepository repository) :
    CommandHandler,
    IRequestHandler<RegistrarClienteCommand, ValidationResult?>,
    IRequestHandler<AdicionarEnderecoCommand, ValidationResult?>
{
    private IClienteRepository Repository { get; } = repository;

    public async Task<ValidationResult?> Handle(RegistrarClienteCommand message, CancellationToken cancellationToken)
    {
        if (!message.EhValido())
            return message.ValidationResult;

        Cliente cliente = new(message.Id, message.Nome, message.Email, message.Cpf);

        Cliente? clienteExistente = await Repository.ObterPorCpf(cliente.Cpf?.Numero!);

        if (clienteExistente is not null)
        {
            AdicionarErro("Este CPF já está em uso.");
            return ValidationResult;
        }

        Repository.Adicionar(cliente);

        cliente.AdicionarEvento(new ClienteRegistradoEvent(message.Id, message.Nome, message.Email, message.Cpf));

        return await PersistirDados(Repository.UnitOfWork);
    }

    public async Task<ValidationResult?> Handle(AdicionarEnderecoCommand message, CancellationToken cancellationToken)
    {
        if (!message.EhValido())
            return message.ValidationResult;

        Endereco endereco = new(
            message.Logradouro,
            message.Numero,
            message.Complemento,
            message.Bairro,
            message.Cep,
            message.Cidade,
            message.Estado,
            message.ClienteId);

        Repository.AdicionarEndereco(endereco);

        return await PersistirDados(Repository.UnitOfWork);
    }
}
