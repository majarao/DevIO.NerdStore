using DevIO.NerdStore.Core.Utils;
using DevIO.NerdStore.MessageBus;
using DevIO.NerdStore.Pedidos.API.Services;

namespace DevIO.NerdStore.Pedidos.API.Configuration;

public static class MessageBusConfig
{
    public static IServiceCollection AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetMessageQueueConnection("MessageBus");

        ArgumentNullException.ThrowIfNull(connectionString);

        services
            .AddMessageBus(connectionString)
            .AddHostedService<PedidoOrquestradorIntegrationHandler>()
            .AddHostedService<PedidoIntegrationHandler>();

        return services;
    }
}