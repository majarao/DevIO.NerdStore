using DevIO.NerdStore.BFF.Compras.Extensions;
using Microsoft.Extensions.Options;

namespace DevIO.NerdStore.BFF.Compras.Services;

public interface IPedidoService
{
}

public class PedidoService : Service, IPedidoService
{
    private HttpClient HttpClient { get; }

    public PedidoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
    {
        HttpClient = httpClient;
        HttpClient.BaseAddress = new Uri(settings.Value.PedidoUrl);
    }
}