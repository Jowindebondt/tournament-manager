using Design.Application.DTOs;
using Design.Application.Interfaces;
using Design.Application.Poules.Queries;
using Design.Application.Rounds.Queries;
using Design.Application.Tournaments.Queries;
using MediatR;

namespace Design.Application.Services;

/// <summary>
/// In-process implementation of <see cref="IDesignModuleApi"/>.
/// Routes calls through the existing MediatR CQRS pipeline so that
/// all cross-cutting concerns (logging, validation, etc.) are applied consistently.
/// When the Design module is extracted into a microservice, replace this class
/// with an HTTP client that implements the same interface.
/// </summary>
public class DesignModuleService(IMediator mediator) : IDesignModuleApi
{
    public async Task<TournamentDto?> GetTournamentAsync(Guid id)
        => await mediator.Send(new GetTournamentByIdQuery(id));

    public async Task<IReadOnlyList<RoundDto>> GetRoundsByTournamentAsync(Guid tournamentId)
    {
        var rounds = await mediator.Send(new GetAllRoundsByTournamentQuery(tournamentId));
        return [.. rounds];
    }

    public async Task<IReadOnlyList<PouleDto>> GetPoulesByRoundAsync(Guid tournamentId, Guid roundId)
    {
        var poules = await mediator.Send(new GetAllPoulesByRoundAndTournamentQuery(roundId, tournamentId));
        return [.. poules];
    }
}
