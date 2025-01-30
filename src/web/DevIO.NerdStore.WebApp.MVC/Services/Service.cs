using DevIO.NerdStore.WebApp.MVC.Extensions;
using DevIO.NerdStore.WebApp.MVC.Models;
using System.Text;
using System.Text.Json;

namespace DevIO.NerdStore.WebApp.MVC.Services;

public abstract class Service
{
    private JsonSerializerOptions JsonSerializerOptions { get; }

    protected Service()
    {
        JsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
    }

    protected static StringContent ObterConteudo(object dado) =>
        new(JsonSerializer.Serialize(dado), Encoding.UTF8, "application/json");

    protected async Task<T?> DeserializarObjetoResponse<T>(HttpResponseMessage? responseMessage)
    {
        if (responseMessage is not null && responseMessage.Content is not null)
        {
            string content = await responseMessage.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(content))
                return JsonSerializer.Deserialize<T>(content, JsonSerializerOptions);
        }

        return default;
    }

    protected static bool TratarErrosResponse(HttpResponseMessage response)
    {
        switch ((int)response.StatusCode)
        {
            case 401:
            case 403:
            case 404:
            case 500:
                throw new CustomHttpRequestException(response.StatusCode);
            case 400:
                return false;
        }

        response.EnsureSuccessStatusCode();
        return true;
    }

    protected ResponseResult RetornoOk() => new();
}
