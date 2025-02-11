using DevIO.NerdStore.Core.Data;
using DevIO.NerdStore.Core.Mediator;
using DevIO.NerdStore.Core.Messages;
using DevIO.NerdStore.Pedido.Domain.Vouchers;
using DevIO.NerdStore.Pedido.Infra.Extensions;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DevIO.NerdStore.Pedido.Infra.Data;

public class PedidosContext(DbContextOptions<PedidosContext> options, IMediatorHandler mediator) : DbContext(options), IUnitOfWork
{
    private IMediatorHandler Mediator { get; } = mediator;

    public DbSet<Voucher> Vouchers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (IMutableProperty? property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        modelBuilder.Ignore<Event>();
        modelBuilder.Ignore<ValidationResult>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PedidosContext).Assembly);

        foreach (IMutableForeignKey? relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.HasSequence<int>("MinhaSequencia").StartsAt(1000).IncrementsBy(1);

        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> Commit()
    {
        foreach (EntityEntry? entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") is not null))
        {
            if (entry.State == EntityState.Added)
                entry.Property("DataCadastro").CurrentValue = DateTime.Now;

            if (entry.State == EntityState.Modified)
                entry.Property("DataCadastro").IsModified = false;
        }

        bool sucesso = await base.SaveChangesAsync() > 0;

        if (sucesso)
            await Mediator.PublicarEventos(this);

        return sucesso;
    }
}
