using Competition.Application.Interfaces;
using Design.Application.Interfaces;
using MediatR;

namespace Generation.Application.Tournaments.Commands;

public record GenerateCompetitionCommand(Guid TournamentId) : IRequest;

public class GenerateCompetitionCommandHandler(
    IDesignModuleApi designModule,
    ICompetitionModuleApi competitionModule)
    : IRequestHandler<GenerateCompetitionCommand>
{
    public async Task Handle(GenerateCompetitionCommand request, CancellationToken cancellationToken)
    {
        var tournament = await designModule.GetTournamentAsync(request.TournamentId)
            ?? throw new ArgumentException("Tournament not found");

        var rounds = await designModule.GetRoundsByTournamentAsync(request.TournamentId);

        var roundCreations = new List<RoundCreationDto>();
        foreach (var designRound in rounds)
        {
            var poules = await designModule.GetPoulesByRoundAsync(request.TournamentId, designRound.Id);
            var pouleCreations = poules
                .Select(p => new PouleCreationDto(p.Name, p.TotalPlayers))
                .ToList();
            roundCreations.Add(new RoundCreationDto(designRound.Name, designRound.Type, pouleCreations));
        }

        await competitionModule.CreateCompetitionAsync(new CompetitionCreationDto(
            tournament.Name,
            tournament.Sport.ToString(),
            roundCreations));
    }
}
