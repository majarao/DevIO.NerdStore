using DevIO.NerdStore.BFF.Compras.Extensions;
using Microsoft.Extensions.Options;

namespace DevIO.NerdStore.BFF.Compras.Services;

public interface IPagamentoService
{
}

public class PagamentoService : Service, IPagamentoService
{
    private HttpClient HttpClient { get; }

    public PagamentoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
    {
        HttpClient = httpClient;
        HttpClient.BaseAddress = new Uri(settings.Value.PagamentoUrl);
    }
}