using DevIO.NerdStore.Clientes.API.Application.Commands;
using DevIO.NerdStore.Clientes.API.Application.Events;
using DevIO.NerdStore.Clientes.API.Data;
using DevIO.NerdStore.Clientes.API.Data.Repository;
using DevIO.NerdStore.Clientes.API.Models;
using DevIO.NerdStore.Core.Mediator;
using DevIO.NerdStore.WebAPI.Core.Usuario;
using FluentValidation.Results;
using MediatR;

namespace DevIO.NerdStore.Clientes.API.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();

        services.AddScoped<IMediatorHandler, MediatorHandler>();

        services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult?>, ClienteCommandHandler>();
        services.AddScoped<IRequestHandler<AdicionarEnderecoCommand, ValidationResult?>, ClienteCommandHandler>();

        services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();

        services.AddScoped<IClienteRepository, ClienteRepository>();

        services.AddScoped<ClientesContext>();

        return services;
    }
}
