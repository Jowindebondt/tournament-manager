using Competition.Domain.Entities;
using Competition.Domain.Interfaces;
using Competition.Domain.ValueObjects;
using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using MediatR;
using CompetitionEntity = Competition.Domain.Entities.Competition;
using CompetitionSport = Competition.Domain.Enums.Sport;

namespace Design.Application.Tournaments.Commands;

public record GenerateTournamentCommand(Guid Id) : IRequest;

public class GenerateTournamentCommandHandler(
    ITournamentRepository tournamentRepository,
    IRoundRepository roundRepository,
    IPouleRepository pouleRepository,
    ICompetitionRepository competitionRepository)
    : IRequestHandler<GenerateTournamentCommand>
{
    public async Task Handle(GenerateTournamentCommand request, CancellationToken cancellationToken)
    {
        var tournament = await tournamentRepository.GetByIdAsync(new TournamentId(request.Id))
            ?? throw new ArgumentException("Tournament not found");

        var competitionId = new Competition.Domain.ValueObjects.CompetitionId(Guid.NewGuid());
        var competitionName = Competition.Domain.ValueObjects.CompetitionName.Create(tournament.Name.Value);
        var competitionSport = tournament.Sport switch
        {
            Design.Domain.Enums.Sport.TableTennis => CompetitionSport.TableTennis,
            _ => throw new ArgumentOutOfRangeException(nameof(tournament.Sport), "Unsupported sport.")
        };

        var competition = new CompetitionEntity(competitionId, competitionName, competitionSport);

        var rounds = await roundRepository.GetAllByTournamentAsync(tournament.Id);
        foreach (var designRound in rounds)
        {
            var roundId = new Competition.Domain.ValueObjects.RoundId(Guid.NewGuid());
            var roundName = Competition.Domain.ValueObjects.RoundName.Create(designRound.Name.Value);
            var round = new Round(roundId, roundName, competitionId);

            var poules = await pouleRepository.GetAllByRoundAndTournamentAsync(tournament.Id, designRound.Id);
            foreach (var designPoule in poules)
            {
                var pouleId = new Competition.Domain.ValueObjects.PouleId(Guid.NewGuid());
                var pouleName = Competition.Domain.ValueObjects.PouleName.Create(designPoule.Name.Value);
                var totalPlayers = Competition.Domain.ValueObjects.PoulePlayersCount.Create(designPoule.TotalPlayers.Value);
                var poule = new Poule(pouleId, pouleName, totalPlayers, roundId);
                round.Poules.Add(poule);
            }

            competition.Rounds.Add(round);
        }

        await competitionRepository.AddAsync(competition);
    }
}

