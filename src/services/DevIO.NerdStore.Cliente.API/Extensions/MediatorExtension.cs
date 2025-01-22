using DevIO.NerdStore.Core.DomainObjects;
using DevIO.NerdStore.Core.Mediator;
using DevIO.NerdStore.Core.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DevIO.NerdStore.Clientes.API.Extensions;

public static class MediatorExtension
{
    public static async Task PublicarEventos<T>(this IMediatorHandler mediator, T dbContext) where T : DbContext
    {
        IEnumerable<EntityEntry<Entity>> domainEntities = dbContext.ChangeTracker
            .Entries<Entity>()
            .Where(entity => entity.Entity.Notificacoes.Count != 0);

        List<Event> domainEvents = domainEntities
            .SelectMany(entity => entity.Entity.Notificacoes)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.LimparEventos());

        IEnumerable<Task> tasks = domainEvents
            .Select(async (domainEvent) => await mediator.PublicarEvento(domainEvent));

        await Task.WhenAll(tasks);
    }
}