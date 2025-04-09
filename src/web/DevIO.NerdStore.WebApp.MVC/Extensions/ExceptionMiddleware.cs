using Grpc.Core;
using Polly.CircuitBreaker;
using System.Net;

namespace DevIO.NerdStore.WebApp.MVC.Extensions;

public class ExceptionMiddleware(RequestDelegate next)
{
    private RequestDelegate Next { get; } = next;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await Next(httpContext);
        }
        catch (CustomHttpRequestException ex)
        {
            HandleRequestExceptionAsync(httpContext, ex.StatusCode);
        }
        catch (BrokenCircuitException)
        {
            HandleCircuitBreakerExceptionAsync(httpContext);
        }
        catch (RpcException ex)
        {
            int statusCode = ex.StatusCode switch
            {
                StatusCode.Internal => 400,
                StatusCode.Unauthenticated => 401,
                StatusCode.PermissionDenied => 403,
                StatusCode.Unimplemented => 404,
                _ => 500
            };

            HttpStatusCode httpStatusCode = Enum.Parse<HttpStatusCode>(statusCode.ToString());

            HandleRequestExceptionAsync(httpContext, httpStatusCode);
        }
    }

    private static void HandleRequestExceptionAsync(HttpContext context, HttpStatusCode statusCode)
    {
        if (statusCode == HttpStatusCode.Unauthorized)
        {
            context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");
            return;
        }

        context.Response.StatusCode = (int)statusCode;
    }

    private static void HandleCircuitBreakerExceptionAsync(HttpContext context) => context.Response.Redirect("/sistema-indisponivel");
}
