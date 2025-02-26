using DevIO.NerdStore.Core.Communication;
using DevIO.NerdStore.WebApp.MVC.Extensions;
using DevIO.NerdStore.WebApp.MVC.Models;
using Microsoft.Extensions.Options;
using System.Net;

namespace DevIO.NerdStore.WebApp.MVC.Services;

public class ClienteService : Service, IClienteService
{
    private HttpClient HttpClient { get; }

    public ClienteService(HttpClient httpClient, IOptions<AppSettings> settings)
    {
        HttpClient = httpClient;
        HttpClient.BaseAddress = new Uri(settings.Value.ClienteUrl);
    }

    public async Task<EnderecoViewModel?> ObterEndereco()
    {
        HttpResponseMessage response = await HttpClient.GetAsync("/cliente/endereco/");

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<EnderecoViewModel>(response);
    }

    public async Task<ResponseResult?> AdicionarEndereco(EnderecoViewModel endereco)
    {
        StringContent enderecoContent = ObterConteudo(endereco);

        HttpResponseMessage response = await HttpClient.PostAsync("/cliente/endereco/", enderecoContent);

        if (!TratarErrosResponse(response))
            return await DeserializarObjetoResponse<ResponseResult>(response);

        return RetornoOk();
    }
}
