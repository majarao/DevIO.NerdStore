using DevIO.NerdStore.Clientes.API.Extensions;
using DevIO.NerdStore.Clientes.API.Models;
using DevIO.NerdStore.Core.Data;
using DevIO.NerdStore.Core.Mediator;
using Microsoft.EntityFrameworkCore;

namespace DevIO.NerdStore.Clientes.API.Data;

public class ClientesContext : DbContext, IUnitOfWork
{
    private IMediatorHandler Mediator { get; }

    public ClientesContext(DbContextOptions<ClientesContext> options, IMediatorHandler mediator) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
        Mediator = mediator;
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientesContext).Assembly);
    }

    public async Task<bool> Commit()
    {
        bool sucesso = await base.SaveChangesAsync() > 0;

        if (sucesso)
            await Mediator.PublicarEventos(this);

        return sucesso;
    }
}
