using DevIO.NerdStore.Pagamentos.API.Data;
using DevIO.NerdStore.Pagamentos.API.Facade;
using DevIO.NerdStore.WebAPI.Core.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevIO.NerdStore.Pagamentos.API.Configuration;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PagamentosContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddControllers();

        services.Configure<PagamentoConfig>(configuration.GetSection("PagamentoConfig"));

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