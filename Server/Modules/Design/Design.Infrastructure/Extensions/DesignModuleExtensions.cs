using Design.Application.Interfaces;
using Design.Application.Services;
using Design.Domain.Interfaces;
using Design.Infrastructure.Persistence;
using Design.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Design.Infrastructure.Extensions;

/// <summary>
/// Registers all Design module services so that Platform.Api (or a future
/// standalone host) only needs a single call to wire up the module.
/// Encapsulating registration here means extracting the module into a
/// microservice requires no changes to its internals.
/// </summary>
public static class DesignModuleExtensions
{
    public static IServiceCollection AddDesignModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DesignConnection");
        services.AddDbContext<DesignDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<ITournamentRepository, TournamentRepository>();
        services.AddScoped<IRoundRepository, RoundRepository>();
        services.AddScoped<IPouleRepository, PouleRepository>();

        services.AddScoped<IDesignModuleApi, DesignModuleService>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(Design.Application.MappingProfile).Assembly));

        services.AddAutoMapper(typeof(Design.Application.MappingProfile));

        return services;
    }
}
