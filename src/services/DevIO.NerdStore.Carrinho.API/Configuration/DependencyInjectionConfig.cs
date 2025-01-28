using DevIO.NerdStore.Carrinho.API.Data;
using DevIO.NerdStore.WebAPI.Core.Usuario;

namespace DevIO.NerdStore.Carrinho.API.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();
        services.AddScoped<CarrinhoContext>();

        return services;
    }
}
