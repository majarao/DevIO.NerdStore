using DevIO.NerdStore.BFF.Compras.Extensions;
using DevIO.NerdStore.BFF.Compras.Models;
using Microsoft.Extensions.Options;
using System.Net;

namespace DevIO.NerdStore.BFF.Compras.Services;

public class ClienteService : Service, IClienteService
{
    private HttpClient HttpClient { get; }

    public ClienteService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
    {
        HttpClient = httpClient;
        HttpClient.BaseAddress = new Uri(settings.Value.ClienteUrl);
    }

    public async Task<EnderecoDTO?> ObterEndereco()
    {
        HttpResponseMessage response = await HttpClient.GetAsync("/cliente/endereco/");

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<EnderecoDTO>(response);
    }
}