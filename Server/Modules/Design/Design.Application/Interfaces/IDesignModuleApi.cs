using Design.Application.DTOs;

namespace Design.Application.Interfaces;

/// <summary>
/// Public facade for the Design module.
/// Other modules and the platform use this interface instead of accessing
/// Design domain types directly, allowing the module to be extracted into
/// a microservice by replacing this in-process implementation with an HTTP client.
/// </summary>
public interface IDesignModuleApi
{
    Task<TournamentDto?> GetTournamentAsync(Guid id);
    Task<IReadOnlyList<RoundDto>> GetRoundsByTournamentAsync(Guid tournamentId);
    Task<IReadOnlyList<PouleDto>> GetPoulesByRoundAsync(Guid tournamentId, Guid roundId);
}
