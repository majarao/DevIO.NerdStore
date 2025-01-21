using DevIO.NerdStore.Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace DevIO.NerdStore.Core.Mediator;

public class MediatorHandler(IMediator mediator) : IMediatorHandler
{
    private IMediator Mediator { get; } = mediator;

    public async Task<ValidationResult> EnviarComando<T>(T comando) where T : Command => await Mediator.Send(comando);

    public async Task PublicarEvento<T>(T evento) where T : Event => await Mediator.Publish(evento);
}
