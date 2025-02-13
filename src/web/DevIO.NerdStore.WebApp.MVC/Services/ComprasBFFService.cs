using DevIO.NerdStore.Core.Communication;
using DevIO.NerdStore.WebApp.MVC.Extensions;
using DevIO.NerdStore.WebApp.MVC.Models;
using Microsoft.Extensions.Options;

namespace DevIO.NerdStore.WebApp.MVC.Services;

public class ComprasBFFService : Service, IComprasBFFService
{
    private HttpClient HttpClient { get; }

    public ComprasBFFService(HttpClient httpClient, IOptions<AppSettings> settings)
    {
        HttpClient = httpClient;
        HttpClient.BaseAddress = new Uri(settings.Value.ComprasBFFUrl);
    }

    public async Task<CarrinhoViewModel?> ObterCarrinho()
    {
        HttpResponseMessage response = await HttpClient.GetAsync("/compras/carrinho/");

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<CarrinhoViewModel>(response);
    }

    public async Task<int> ObterQuantidadeCarrinho()
    {
        HttpResponseMessage response = await HttpClient.GetAsync("/compras/carrinho-quantidade/");

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<int>(response);
    }

    public async Task<ResponseResult?> AdicionarItemCarrinho(ItemCarrinhoViewModel carrinho)
    {
        StringContent itemContent = ObterConteudo(carrinho);

        HttpResponseMessage response = await HttpClient.PostAsync("/compras/carrinho/items/", itemContent);

        if (!TratarErrosResponse(response))
            return await DeserializarObjetoResponse<ResponseResult>(response);

        return RetornoOk();
    }

    public async Task<ResponseResult?> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel item)
    {
        StringContent itemContent = ObterConteudo(item);

        HttpResponseMessage response = await HttpClient.PutAsync($"/compras/carrinho/items/{produtoId}", itemContent);

        if (!TratarErrosResponse(response))
            return await DeserializarObjetoResponse<ResponseResult>(response);

        return RetornoOk();
    }

    public async Task<ResponseResult?> RemoverItemCarrinho(Guid produtoId)
    {
        HttpResponseMessage response = await HttpClient.DeleteAsync($"/compras/carrinho/items/{produtoId}");

        if (!TratarErrosResponse(response))
            return await DeserializarObjetoResponse<ResponseResult>(response);

        return RetornoOk();
    }

    public async Task<ResponseResult?> AplicarVoucherCarrinho(string voucher)
    {
        StringContent itemContent = ObterConteudo(voucher);

        HttpResponseMessage response = await HttpClient.PostAsync("/compras/carrinho/aplicar-voucher/", itemContent);

        if (!TratarErrosResponse(response))
            return await DeserializarObjetoResponse<ResponseResult>(response);

        return RetornoOk();
    }
}
