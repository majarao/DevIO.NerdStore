using DevIO.NerdStore.WebAPI.Core.Usuario;

namespace DevIO.NerdStore.BFF.Compras.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<IAspNetUser, AspNetUser>();

        return services;
    }
}
