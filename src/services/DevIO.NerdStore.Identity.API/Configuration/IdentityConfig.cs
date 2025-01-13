using DevIO.NerdStore.Identity.API.Data;
using DevIO.NerdStore.Identity.API.Extensions;
using DevIO.NerdStore.WebAPI.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevIO.NerdStore.Identity.API.Configuration;

public static class IdentityConfig
{
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDBContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services
            .AddDefaultIdentity<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDBContext>()
            .AddDefaultTokenProviders()
            .AddErrorDescriber<IdentityMensagensPortugues>();

        services.AddJwtConfiguration(configuration);

        return services;
    }
}
