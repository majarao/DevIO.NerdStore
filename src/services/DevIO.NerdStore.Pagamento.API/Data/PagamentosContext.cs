using DevIO.NerdStore.Core.Data;
using DevIO.NerdStore.Core.Messages;
using DevIO.NerdStore.Pagamentos.API.Models;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DevIO.NerdStore.Pagamentos.API.Data;

public sealed class PagamentosContext : DbContext, IUnitOfWork
{
    public PagamentosContext(DbContextOptions<PagamentosContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Pagamento> Pagamentos { get; set; }
    public DbSet<Transacao> Transacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<ValidationResult>();
        modelBuilder.Ignore<Event>();

        foreach (IMutableProperty? property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        foreach (IMutableForeignKey? relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PagamentosContext).Assembly);
    }

    public async Task<bool> Commit() => await SaveChangesAsync() > 0;
}