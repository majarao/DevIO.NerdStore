using DevIO.NerdStore.Clientes.API.Services;
using DevIO.NerdStore.Core.Utils;
using DevIO.NerdStore.MessageBus;

namespace DevIO.NerdStore.Clientes.API.Configuration;

public static class MessageBusConfig
{
    public static IServiceCollection AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetMessageQueueConnnection("MessageBus");

        ArgumentNullException.ThrowIfNull(connectionString);

        services
            .AddMessageBus(connectionString)
            .AddHostedService<RegistroClienteIntegrationHandler>();

        return services;
    }
}
