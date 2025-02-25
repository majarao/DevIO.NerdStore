using DevIO.NerdStore.Carrinho.API.Services;
using DevIO.NerdStore.Core.Utils;
using DevIO.NerdStore.MessageBus;

namespace DevIO.NerdStore.Carrinho.API.Configuration;

public static class MessageBusConfig
{
    public static IServiceCollection AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetMessageQueueConnection("MessageBus");

        ArgumentNullException.ThrowIfNull(connectionString);

        services
            .AddMessageBus(connectionString)
            .AddHostedService<CarrinhoIntegrationHandler>();

        return services;
    }
}