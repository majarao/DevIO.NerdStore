using DevIO.NerdStore.Clientes.API.Application.Commands;
using DevIO.NerdStore.Core.Mediator;
using DevIO.NerdStore.Core.Messages.Integration;
using DevIO.NerdStore.MessageBus;
using FluentValidation.Results;

namespace DevIO.NerdStore.Clientes.API.Services;

public class RegistroClienteIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus) : BackgroundService
{
    private IServiceProvider ServiceProvider { get; } = serviceProvider;
    private IMessageBus Bus { get; } = bus;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Bus.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(async request => await RegistrarCliente(request));

        return Task.CompletedTask;
    }

    private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistradoIntegrationEvent message)
    {
        RegistrarClienteCommand clienteCommand = new(message.Id, message.Nome, message.Email, message.Cpf);

        ValidationResult resultado;

        using (IServiceScope scope = ServiceProvider.CreateScope())
        {
            IMediatorHandler mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();

            resultado = await mediator.EnviarComando(clienteCommand);
        }

        return new(resultado);
    }
}
