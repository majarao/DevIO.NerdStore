using Microsoft.Extensions.Configuration;

namespace DevIO.NerdStore.Core.Utils;

public static class ConfigurationExtensions
{
    public static string? GetMessageQueueConnection(this IConfiguration configuration, string name) =>
        configuration?.GetSection("MessageQueueConnnection")?[name];
}
