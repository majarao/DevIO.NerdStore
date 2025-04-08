using DevIO.NerdStore.BFF.Compras.Configuration;
using DevIO.NerdStore.WebAPI.Core.Identity;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IConfigurationBuilder builderConfiguration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

if (builder.Environment.IsDevelopment())
    builderConfiguration.AddUserSecrets<Program>();

IConfiguration Configuration = builderConfiguration.Build();

builder.Services
    .AddApiConfiguration(Configuration)
    .ConfigureGrpcServices(Configuration)
    .AddJwtConfiguration(Configuration)
    .AddSwaggerConfiguration()
    .RegisterServices()
    .AddMessageBusConfiguration(Configuration);

WebApplication app = builder.Build();

app
    .UseSwaggerConfiguration()
    .UseApiConfiguration(builder.Environment);

app.Run();
