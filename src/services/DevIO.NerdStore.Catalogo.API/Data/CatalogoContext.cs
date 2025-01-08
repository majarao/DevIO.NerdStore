using DevIO.NerdStore.Catalogo.API.Models;
using DevIO.NerdStore.Core.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DevIO.NerdStore.Catalogo.API.Data;

public class CatalogoContext(DbContextOptions<CatalogoContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Produto> Produtos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public async Task<bool> Commit() => await base.SaveChangesAsync() > 0;
}
