using Microsoft.Extensions.DependencyInjection;

namespace DevIO.NerdStore.MessageBus;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMessageBus(this IServiceCollection services, string connection)
    {
        ArgumentNullException.ThrowIfNull(connection);

        services.AddSingleton<IMessageBus>(new MessageBus(connection));

        return services;
    }
}
