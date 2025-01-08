using DevIO.NerdStore.Catalogo.API.Data.Repositories;
using DevIO.NerdStore.Catalogo.API.Data;
using DevIO.NerdStore.Catalogo.API.Models;

namespace DevIO.NerdStore.Catalogo.API.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<CatalogoContext>();

        return services;
    }
}
