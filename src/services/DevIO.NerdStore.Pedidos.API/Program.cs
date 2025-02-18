using DevIO.NerdStore.Pedidos.API.Configuration;
using DevIO.NerdStore.WebAPI.Core.Identity;
using System.Reflection;

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
    .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()))
    .AddApiConfiguration(Configuration)
    .AddJwtConfiguration(Configuration)
    .RegisterServices()
    .AddSwaggerConfiguration()
    .AddMessageBusConfiguration(Configuration);

WebApplication app = builder.Build();

app
    .UseApiConfiguration(builder.Environment)
    .UseSwaggerConfiguration();

app.Run();
