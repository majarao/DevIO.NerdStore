using DevIO.NerdStore.BFF.Compras.Extensions;
using DevIO.NerdStore.BFF.Compras.Models;
using Microsoft.Extensions.Options;
using System.Net;

namespace DevIO.NerdStore.BFF.Compras.Services;

public class PedidoService : Service, IPedidoService
{
    private HttpClient HttpClient { get; }

    public PedidoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
    {
        HttpClient = httpClient;
        HttpClient.BaseAddress = new Uri(settings.Value.PedidoUrl);
    }

    public async Task<VoucherDTO?> ObterVoucherPorCodigo(string codigo)
    {
        HttpResponseMessage response = await HttpClient.GetAsync($"/voucher/{codigo}/");

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<VoucherDTO>(response);
    }
}