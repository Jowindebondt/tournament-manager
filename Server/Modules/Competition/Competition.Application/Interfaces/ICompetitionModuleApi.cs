namespace Competition.Application.Interfaces;

/// <summary>
/// Public facade for the Competition module.
/// Other modules and the platform use this interface instead of accessing
/// Competition domain types directly, allowing the module to be extracted into
/// a microservice by replacing this in-process implementation with an HTTP client.
/// </summary>
public interface ICompetitionModuleApi
{
    Task CreateCompetitionAsync(CompetitionCreationDto dto);
}

public record CompetitionCreationDto(
    string Name,
    /// <summary>Sport name matching the <c>Competition.Domain.Enums.Sport</c> enum (e.g. "TableTennis").</summary>
    string Sport,
    IReadOnlyList<RoundCreationDto> Rounds);

public record RoundCreationDto(
    string Name,
    /// <summary>
    /// Round plan type string. Supported values:
    /// <list type="bullet">
    ///   <item><c>null</c> – no plan</item>
    ///   <item><c>"RoundRobin"</c></item>
    ///   <item><c>"KnockOut:{Phase}"</c> where Phase is a <c>KnockOutPhase</c> enum name
    ///   (e.g. <c>"KnockOut:Final"</c>, <c>"KnockOut:SemiFinal"</c>).</item>
    /// </list>
    /// </summary>
    string? PlanType,
    IReadOnlyList<PouleCreationDto> Poules);

public record PouleCreationDto(
    string Name,
    int TotalPlayers);
