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
    string Sport,
    IReadOnlyList<RoundCreationDto> Rounds);

public record RoundCreationDto(
    string Name,
    string? PlanType,
    IReadOnlyList<PouleCreationDto> Poules);

public record PouleCreationDto(
    string Name,
    int TotalPlayers);
