using DevIO.NerdStore.BFF.Compras.Extensions;
using DevIO.NerdStore.BFF.Compras.Models;
using DevIO.NerdStore.Core.Communication;
using Microsoft.Extensions.Options;

namespace DevIO.NerdStore.BFF.Compras.Services;

public class CarrinhoService : Service, ICarrinhoService
{
    private HttpClient HttpClient { get; }

    public CarrinhoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
    {
        HttpClient = httpClient;
        HttpClient.BaseAddress = new Uri(settings.Value.CarrinhoUrl);
    }

    public async Task<CarrinhoDTO?> ObterCarrinho()
    {
        HttpResponseMessage response = await HttpClient.GetAsync("/carrinho/");

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<CarrinhoDTO>(response);
    }

    public async Task<ResponseResult?> AdicionarItemCarrinho(ItemCarrinhoDTO produto)
    {
        StringContent itemContent = ObterConteudo(produto);

        HttpResponseMessage response = await HttpClient.PostAsync("/carrinho/", itemContent);

        if (!TratarErrosResponse(response))
            return await DeserializarObjetoResponse<ResponseResult>(response);

        return RetornoOk();
    }

    public async Task<ResponseResult?> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDTO carrinho)
    {
        StringContent itemContent = ObterConteudo(carrinho);

        HttpResponseMessage response = await HttpClient.PutAsync($"/carrinho/{carrinho.ProdutoId}", itemContent);

        if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

        return RetornoOk();
    }

    public async Task<ResponseResult?> RemoverItemCarrinho(Guid produtoId)
    {
        HttpResponseMessage response = await HttpClient.DeleteAsync($"/carrinho/{produtoId}");

        if (!TratarErrosResponse(response))
            return await DeserializarObjetoResponse<ResponseResult>(response);

        return RetornoOk();
    }
}
