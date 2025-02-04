using DevIO.NerdStore.BFF.Compras.Extensions;
using Microsoft.Extensions.Options;

namespace DevIO.NerdStore.BFF.Compras.Services;

public interface ICatalogoService
{
}

public class CatalogoService : Service, ICatalogoService
{
    private HttpClient HttpClient { get; }

    public CatalogoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
    {
        HttpClient = httpClient;
        HttpClient.BaseAddress = new Uri(settings.Value.CatalogoUrl);
    }
}