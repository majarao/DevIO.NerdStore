using DevIO.NerdStore.BFF.Compras.Extensions;
using DevIO.NerdStore.BFF.Compras.Models;
using Microsoft.Extensions.Options;

namespace DevIO.NerdStore.BFF.Compras.Services;

public class CatalogoService : Service, ICatalogoService
{
    private HttpClient HttpClient { get; }

    public CatalogoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
    {
        HttpClient = httpClient;
        HttpClient.BaseAddress = new Uri(settings.Value.CatalogoUrl);
    }

    public async Task<ItemProdutoDTO?> ObterPorId(Guid id)
    {
        HttpResponseMessage response = await HttpClient.GetAsync($"/catalogo/produtos/{id}");

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<ItemProdutoDTO>(response);
    }
}