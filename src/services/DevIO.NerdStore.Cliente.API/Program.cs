WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

WebApplication app = builder.Build();

app.MapOpenApi();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Cliente API"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
