using DevIO.NerdStore.Core.Utils;
using DevIO.NerdStore.MessageBus;
using DevIO.NerdStore.Pagamentos.API.Services;

namespace DevIO.NerdStore.Pagamentos.API.Configuration;

public static class MessageBusConfig
{
    public static IServiceCollection AddMessageBusConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        string? connectionString = configuration.GetMessageQueueConnection("MessageBus");

        ArgumentNullException.ThrowIfNull(connectionString);

        services
            .AddMessageBus(connectionString)
            .AddHostedService<PagamentoIntegrationHandler>();

        return services;
    }
}