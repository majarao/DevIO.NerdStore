using DevIO.NerdStore.Core.Mediator;
using DevIO.NerdStore.Pedido.API.Application.Queries;
using DevIO.NerdStore.Pedido.Domain.Vouchers;
using DevIO.NerdStore.Pedido.Infra.Data;
using DevIO.NerdStore.Pedido.Infra.Data.Repository;
using DevIO.NerdStore.WebAPI.Core.Usuario;

namespace DevIO.NerdStore.Pedido.API.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<PedidosContext>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();

        services.AddScoped<IMediatorHandler, MediatorHandler>();

        services.AddScoped<IVoucherRepository, VoucherRepository>();

        services.AddScoped<IVoucherQueries, VoucherQueries>();

        return services;
    }
}
