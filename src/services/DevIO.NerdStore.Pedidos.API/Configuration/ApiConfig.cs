using DevIO.NerdStore.Pedidos.Infra.Data;
using DevIO.NerdStore.WebAPI.Core.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevIO.NerdStore.Pedidos.API.Configuration;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PedidosContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddPolicy("Total",
                builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
        });

        return services;
    }

    public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors("Total");

        app.UseAuthConfiguration();

        app.UseEndpoints(endpoints => endpoints.MapControllers());

        return app;
    }
}
