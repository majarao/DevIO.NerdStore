using DevIO.NerdStore.Catalogo.API.Configuration;

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
    .RegisterServices()
    .AddSwaggerConfiguration();

WebApplication app = builder.Build();

app
    .UseApiConfiguration(builder.Environment)
    .UseSwaggerConfiguration();

app.Run();
