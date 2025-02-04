using DevIO.NerdStore.BFF.Compras.Extensions;
using Microsoft.Extensions.Options;

namespace DevIO.NerdStore.BFF.Compras.Services;

public interface ICarrinhoService
{
}

public class CarrinhoService : Service, ICarrinhoService
{
    private HttpClient HttpClient { get; }

    public CarrinhoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
    {
        HttpClient = httpClient;
        HttpClient.BaseAddress = new Uri(settings.Value.CarrinhoUrl);
    }
}
