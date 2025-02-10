using DevIO.NerdStore.Core.Mediator;
using DevIO.NerdStore.Core.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using DevIO.NerdStore.Core.DomainObjects;

namespace DevIO.NerdStore.Pedido.Infra.Extensions;

public static class MediatorExtension
{
    public static async Task PublicarEventos<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
    {
        IEnumerable<EntityEntry<Entity>> domainEntities = ctx.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());

        List<Event> domainEvents = domainEntities
            .SelectMany(x => x.Entity.Notificacoes)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.LimparEventos());

        IEnumerable<Task> tasks = domainEvents
            .Select(async (domainEvent) => await mediator.PublicarEvento(domainEvent));

        await Task.WhenAll(tasks);
    }
}