using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Primitives;

namespace DevIO.NerdStore.BFF.Compras.Services.gRPC;

public class GrpcServiceInterceptor(IHttpContextAccessor httpContextAccesor) : Interceptor
{
    private IHttpContextAccessor Accesor { get; } = httpContextAccesor;

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        StringValues token = Accesor!.HttpContext!.Request.Headers.Authorization;

        Metadata headers = new()
        {
            {"Authorization", token!}
        };

        CallOptions options = context.Options.WithHeaders(headers);

        context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, options);

        return base.AsyncUnaryCall(request, context, continuation);
    }
}
