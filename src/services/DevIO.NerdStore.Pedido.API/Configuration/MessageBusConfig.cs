using DevIO.NerdStore.Core.Utils;
using DevIO.NerdStore.MessageBus;

namespace DevIO.NerdStore.Pedido.API.Configuration;

public static class MessageBusConfig
{
    public static IServiceCollection AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetMessageQueueConnection("MessageBus");

        ArgumentNullException.ThrowIfNull(connectionString);

        services
            .AddMessageBus(connectionString);

        return services;
    }
}