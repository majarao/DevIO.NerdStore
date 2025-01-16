using DevIO.NerdStore.WebApp.MVC.Configuration;

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
    .AddIdentityConfiguration()
    .AddMvcConfiguration(Configuration)
    .RegisterServices(Configuration);

WebApplication app = builder.Build();

app.UseMvcConfiguration(app.Environment);

app.Run();
