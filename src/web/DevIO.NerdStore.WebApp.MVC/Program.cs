using DevIO.NerdStore.WebApp.MVC.Configuration;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddIdentityConfiguration()
    .AddMvcConfiguration();

WebApplication app = builder.Build();

app.UseMvcConfiguration(app.Environment);

app.Run();
