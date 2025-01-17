﻿using DevIO.NerdStore.WebAPI.Core.Identity;

namespace DevIO.NerdStore.Identity.API.Configuration;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
    {
        services.AddControllers();

        return services;
    }

    public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app)
    {
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthConfiguration();

        app.UseEndpoints(endpoints => endpoints.MapControllers());

        return app;
    }
}
