using DevIO.NerdStore.WebAPI.Core.Usuario;
using Microsoft.Extensions.Primitives;
using System.Net.Http.Headers;

namespace DevIO.NerdStore.BFF.Compras.Extensions;

public class HttpClientAuthorizationDelegatingHandler(IAspNetUser user) : DelegatingHandler
{
    private IAspNetUser User { get; } = user;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        StringValues authorizationHeader = User.ObterHttpContext()?.Request.Headers.Authorization ?? [];

        if (!string.IsNullOrEmpty(authorizationHeader))
            request.Headers.Add("Authorization", [authorizationHeader]);

        string? token = User.ObterUserToken();

        if (token is not null)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return base.SendAsync(request, cancellationToken);
    }
}
