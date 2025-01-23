using DevIO.NerdStore.WebApp.MVC.Extensions;
using DevIO.NerdStore.WebApp.MVC.Services;
using DevIO.NerdStore.WebApp.MVC.Services.Handlers;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Polly;
using Refit;

namespace DevIO.NerdStore.WebApp.MVC.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();

        services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

        services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();

        services
            .AddHttpClient("Catalogo", options => options.BaseAddress = new Uri(configuration.GetSection("CatalogoUrl").Value ?? string.Empty))
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .AddTypedClient(RestService.For<ICatalogoService>)
            .AddPolicyHandler(PollyExtensions.EsperarTentar())
            .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<IUser, AspNetUser>();

        return services;
    }
}
