using DevIO.NerdStore.Core.Mediator;
using DevIO.NerdStore.Pedidos.API.Application.Queries;
using DevIO.NerdStore.Pedidos.Domain.Pedidos;
using DevIO.NerdStore.Pedidos.Domain.Vouchers;
using DevIO.NerdStore.Pedidos.Infra.Data;
using DevIO.NerdStore.Pedidos.Infra.Data.Repository;
using DevIO.NerdStore.WebAPI.Core.Usuario;

namespace DevIO.NerdStore.Pedidos.API.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<PedidosContext>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();

        services.AddScoped<IMediatorHandler, MediatorHandler>();

        services.AddScoped<IVoucherRepository, VoucherRepository>();
        services.AddScoped<IPedidoRepository, PedidoRepository>();

        services.AddScoped<IVoucherQueries, VoucherQueries>();

        return services;
    }
}
