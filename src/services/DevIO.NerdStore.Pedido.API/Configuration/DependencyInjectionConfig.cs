using DevIO.NerdStore.Core.Mediator;
using DevIO.NerdStore.Pedido.Infra.Data;
using DevIO.NerdStore.WebAPI.Core.Usuario;

namespace DevIO.NerdStore.Pedido.API.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();

        services.AddScoped<IMediatorHandler, MediatorHandler>();

        services.AddScoped<PedidosContext>();

        return services;
    }
}
