using DevIO.NerdStore.Clientes.API.Application.Commands;
using DevIO.NerdStore.Core.Mediator;
using DevIO.NerdStore.Core.Messages.Integration;
using EasyNetQ;
using FluentValidation.Results;

namespace DevIO.NerdStore.Clientes.API.Services;

public class RegistroClienteIntegrationHandler(IServiceProvider serviceProvider) : BackgroundService
{
    private IServiceProvider ServiceProvider { get; } = serviceProvider;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        IBus bus = RabbitHutch.CreateBus("host=localhost:5672", s => s.EnableSystemTextJson());

        bus.Rpc.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(async request => new ResponseMessage(await RegistrarCliente(request)), stoppingToken);

        return Task.CompletedTask;
    }

    private async Task<ValidationResult> RegistrarCliente(UsuarioRegistradoIntegrationEvent message)
    {
        RegistrarClienteCommand clienteCommand = new(message.Id, message.Nome, message.Email, message.Cpf);

        ValidationResult resultado;

        using (IServiceScope scope = ServiceProvider.CreateScope())
        {
            IMediatorHandler mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();

            resultado = await mediator.EnviarComando(clienteCommand);
        }

        return resultado;
    }
}
