using DevIO.NerdStore.Catalogo.API.Services;
using DevIO.NerdStore.Core.Utils;
using DevIO.NerdStore.MessageBus;

namespace DevIO.NerdStore.Catalogo.API.Configuration;

public static class MessageBusConfig
{
    public static IServiceCollection AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetMessageQueueConnection("MessageBus");

        ArgumentNullException.ThrowIfNull(connectionString);

        services
            .AddMessageBus(connectionString)
            .AddHostedService<CatalogoIntegrationHandler>();

        return services;
    }
}