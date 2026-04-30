using Microsoft.Extensions.DependencyInjection;

namespace Generation.Infrastructure.Extensions;

/// <summary>
/// Registers all Generation module services.
/// The Generation module has no dedicated database or repositories —
/// it orchestrates Design and Competition modules through their facades.
/// When the Generation module is extracted into a microservice, its host
/// project calls this extension (or an equivalent) during startup.
/// </summary>
public static class GenerationModuleExtensions
{
    public static IServiceCollection AddGenerationModule(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(Generation.Application.MappingProfile).Assembly));

        return services;
    }
}
