using DevIO.NerdStore.Clientes.API.Application.Commands;
using DevIO.NerdStore.Clientes.API.Data;
using DevIO.NerdStore.Clientes.API.Data.Repository;
using DevIO.NerdStore.Clientes.API.Models;
using DevIO.NerdStore.Core.Mediator;
using FluentValidation.Results;
using MediatR;

namespace DevIO.NerdStore.Clientes.API.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult?>, ClienteCommandHandler>();

        services.AddScoped<IClienteRepository, ClienteRepository>();

        services.AddScoped<ClientesContext>();

        return services;
    }
}
