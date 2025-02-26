using DevIO.NerdStore.BFF.Compras.Extensions;
using DevIO.NerdStore.BFF.Compras.Models;
using DevIO.NerdStore.Core.Communication;
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

    public async Task<ResponseResult?> FinalizarPedido(PedidoDTO pedido)
    {
        StringContent pedidoContent = ObterConteudo(pedido);

        HttpResponseMessage response = await HttpClient.PostAsync("/pedido/", pedidoContent);

        if (!TratarErrosResponse(response))
            return await DeserializarObjetoResponse<ResponseResult>(response);

        return RetornoOk();
    }

    public async Task<PedidoDTO?> ObterUltimoPedido()
    {
        HttpResponseMessage response = await HttpClient.GetAsync("/pedido/ultimo/");

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<PedidoDTO>(response);
    }

    public async Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId()
    {
        HttpResponseMessage response = await HttpClient.GetAsync("/pedido/lista-cliente/");

        if (response.StatusCode == HttpStatusCode.NotFound)
            return [];

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<IEnumerable<PedidoDTO>>(response) ?? [];
    }
}