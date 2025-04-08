using DevIO.NerdStore.BFF.Compras.Services.gRPC;
using DevIO.NerdStore.Carrinho.API.Services.gRPC;

namespace DevIO.NerdStore.BFF.Compras.Configuration;

public static class GrpcConfig
{
    public static IServiceCollection ConfigureGrpcServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<GrpcServiceInterceptor>();

        services.AddScoped<ICarrinhoGrpcService, CarrinhoGrpcService>();

        services.AddGrpcClient<CarrinhoCompras.CarrinhoComprasClient>(options =>
        {
            string? carrinhoUrl = configuration["CarrinhoUrl"];

            ArgumentNullException.ThrowIfNull(carrinhoUrl);

            options.Address = new Uri(carrinhoUrl);
        })
            .AddInterceptor<GrpcServiceInterceptor>();

        return services;
    }
}