using DevIO.NerdStore.Pagamentos.API.Data;
using DevIO.NerdStore.Pagamentos.API.Data.Repository;
using DevIO.NerdStore.Pagamentos.API.Facade;
using DevIO.NerdStore.Pagamentos.API.Models;
using DevIO.NerdStore.Pagamentos.API.Services;
using DevIO.NerdStore.WebAPI.Core.Usuario;

namespace DevIO.NerdStore.Pagamentos.API.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();

        services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        services.AddScoped<IPagamentoService, PagamentoService>();
        services.AddScoped<IPagamentoFacade, PagamentoCartaoCreditoFacade>();

        services.AddScoped<PagamentosContext>();

        return services;
    }
}