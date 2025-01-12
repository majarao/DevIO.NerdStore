namespace DevIO.NerdStore.Identity.API.Configuration;

public static class SwaggerConfig
{
    public static IServiceCollection AddDocumentacaoConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen();

        return services;
    }

    public static IApplicationBuilder UseDocumentacaoConfiguration(this IApplicationBuilder app)
    {
        app.UseSwagger();

        app.UseSwaggerUI();

        return app;
    }
}
