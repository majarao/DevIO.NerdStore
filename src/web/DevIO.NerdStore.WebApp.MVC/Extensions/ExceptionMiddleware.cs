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
            HandleRequestExceptionAsync(httpContext, ex);
        }
    }

    private static void HandleRequestExceptionAsync(HttpContext context, CustomHttpRequestException httpRequestException)
    {
        if (httpRequestException.StatusCode == HttpStatusCode.Unauthorized)
        {
            context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");
            return;
        }

        context.Response.StatusCode = (int)httpRequestException.StatusCode;
    }
}
