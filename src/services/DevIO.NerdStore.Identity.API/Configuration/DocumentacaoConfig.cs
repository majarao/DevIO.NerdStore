namespace DevIO.NerdStore.Identity.API.Configuration;

public static class DocumentacaoConfig
{
    public static IServiceCollection AddDocumentacaoConfiguration(this IServiceCollection services)
    {
        services.AddOpenApi();

        return services;
    }

    public static IApplicationBuilder UseDocumentacaoConfiguration(this IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints => endpoints.MapOpenApi());
        app.UseSwaggerUI(
            options => options.SwaggerEndpoint("/openapi/v1.json", "Identity API"));

        return app;
    }
}
