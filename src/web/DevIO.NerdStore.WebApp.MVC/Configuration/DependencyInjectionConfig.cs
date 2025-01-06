using DevIO.NerdStore.WebApp.MVC.Extensions;
using DevIO.NerdStore.WebApp.MVC.Services;

namespace DevIO.NerdStore.WebApp.MVC.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUser, AspNetUser>();

        return services;
    }
}
