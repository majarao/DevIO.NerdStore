using DevIO.NerdStore.WebApp.MVC.Extensions;
using DevIO.NerdStore.WebApp.MVC.Models;
using Microsoft.Extensions.Options;

namespace DevIO.NerdStore.WebApp.MVC.Services;

public class CarrinhoService : Service, ICarrinhoService
{
    private HttpClient HttpClient { get; }

    public CarrinhoService(HttpClient httpClient, IOptions<AppSettings> settings)
    {
        HttpClient = httpClient;
        HttpClient.BaseAddress = new Uri(settings.Value.CarrinhoUrl);
    }

    public async Task<CarrinhoViewModel?> ObterCarrinho()
    {
        HttpResponseMessage response = await HttpClient.GetAsync("/carrinho/");

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<CarrinhoViewModel>(response);
    }

    public async Task<ResponseResult?> AdicionarItemCarrinho(ItemProdutoViewModel produto)
    {
        StringContent itemContent = ObterConteudo(produto);

        HttpResponseMessage response = await HttpClient.PostAsync("/carrinho/", itemContent);

        if (!TratarErrosResponse(response))
            return await DeserializarObjetoResponse<ResponseResult>(response);

        return RetornoOk();
    }

    public async Task<ResponseResult?> AtualizarItemCarrinho(Guid produtoId, ItemProdutoViewModel produto)
    {
        StringContent itemContent = ObterConteudo(produto);

        HttpResponseMessage response = await HttpClient.PutAsync($"/carrinho/{produto.ProdutoId}", itemContent);

        if (!TratarErrosResponse(response))
            return await DeserializarObjetoResponse<ResponseResult>(response);

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
