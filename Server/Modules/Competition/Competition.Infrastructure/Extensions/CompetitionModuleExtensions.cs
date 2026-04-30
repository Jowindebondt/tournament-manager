using Competition.Application.Interfaces;
using Competition.Application.Services;
using Competition.Domain.Interfaces;
using Competition.Infrastructure.Persistence;
using Competition.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Competition.Infrastructure.Extensions;

/// <summary>
/// Registers all Competition module services so that Platform.Api (or a future
/// standalone host) only needs a single call to wire up the module.
/// Encapsulating registration here means extracting the module into a
/// microservice requires no changes to its internals.
/// </summary>
public static class CompetitionModuleExtensions
{
    public static IServiceCollection AddCompetitionModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CompetitionConnection");
        services.AddDbContext<CompetitionDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<ICompetitionRepository, CompetitionRepository>();
        services.AddScoped<IMatchRepository, MatchRepository>();

        services.AddScoped<ICompetitionModuleApi, CompetitionModuleService>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(Competition.Application.MappingProfile).Assembly));

        services.AddAutoMapper(typeof(Competition.Application.MappingProfile));

        return services;
    }
}
