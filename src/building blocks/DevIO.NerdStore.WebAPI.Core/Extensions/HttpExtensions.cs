using Microsoft.Extensions.DependencyInjection;

namespace DevIO.NerdStore.WebAPI.Core.Extensions;

public static class HttpExtensions
{
    public static IHttpClientBuilder AllowSelfSignedCertificate(this IHttpClientBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        return builder.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });
    }
}