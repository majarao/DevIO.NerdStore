using DevIO.NerdStore.Catalogo.API.Data;
using DevIO.NerdStore.Catalogo.API.Data.Repositories;
using DevIO.NerdStore.Catalogo.API.Models;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CatalogoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<CatalogoContext>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

WebApplication app = builder.Build();

app.MapOpenApi();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Catalogo API"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
