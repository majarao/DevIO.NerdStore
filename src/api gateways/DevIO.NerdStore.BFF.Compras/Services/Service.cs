using System.Net;
using System.Text;
using System.Text.Json;

namespace DevIO.NerdStore.BFF.Compras.Services;

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
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return false;

        response.EnsureSuccessStatusCode();
        return true;
    }
}
