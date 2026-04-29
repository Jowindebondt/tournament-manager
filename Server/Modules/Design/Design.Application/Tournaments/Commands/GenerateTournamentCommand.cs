using Competition.Domain.Enums;
using Competition.Domain.Interfaces;
using Competition.Domain.ValueObjects;
using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using MediatR;

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

        var designRounds = await roundRepository.GetAllByTournamentAsync(new TournamentId(request.Id));

        var competitionId = new CompetitionId(Guid.NewGuid());
        var competitionName = CompetitionName.Create(tournament.Name.Value);
        var competitionSport = MapSport(tournament.Sport);
        var competition = new Competition.Domain.Entities.Competition(competitionId, competitionName, competitionSport);

        foreach (var designRound in designRounds)
        {
            var competitionRoundId = new CompetitionRoundId(Guid.NewGuid());
            var competitionRoundName = CompetitionRoundName.Create(designRound.Name.Value);
            var competitionRound = new Competition.Domain.Entities.CompetitionRound(
                competitionRoundId, competitionRoundName, competitionId);

            if (designRound.Type != null)
            {
                competitionRound.SetPlan(MapToPlan(designRound.Type));
            }

            var designPoules = await pouleRepository.GetAllByRoundAndTournamentAsync(
                new TournamentId(request.Id), designRound.Id);

            foreach (var designPoule in designPoules)
            {
                var competitionPouleId = new CompetitionPouleId(Guid.NewGuid());
                var competitionPouleName = CompetitionPouleName.Create(designPoule.Name.Value);
                var competitionPoule = new Competition.Domain.Entities.CompetitionPoule(
                    competitionPouleId, competitionPouleName, competitionRoundId);

                competitionRound.Poules.Add(competitionPoule);
            }

            competition.Rounds.Add(competitionRound);
        }

        await competitionRepository.AddAsync(competition);
    }

    private static Sport MapSport(Design.Domain.Enums.Sport sport) => sport switch
    {
        Design.Domain.Enums.Sport.TableTennis => Sport.TableTennis,
        _ => throw new ArgumentOutOfRangeException(nameof(sport), $"Unsupported sport: {sport}")
    };

    private static CompetitionPlan MapToPlan(Design.Domain.ValueObjects.RoundType roundType) => roundType switch
    {
        RoundRobinType => new RoundRobinPlan(),
        KnockOutType ko => new KnockOutPlan(MapKnockOutPhase(ko.Phase)),
        _ => throw new ArgumentOutOfRangeException(nameof(roundType), $"Unsupported round type: {roundType.GetType().Name}")
    };

    private static KnockOutPhase MapKnockOutPhase(Design.Domain.Enums.KnockOutPhase phase) => phase switch
    {
        Design.Domain.Enums.KnockOutPhase.SemiFinal => KnockOutPhase.SemiFinal,
        Design.Domain.Enums.KnockOutPhase.ThirdPlace => KnockOutPhase.ThirdPlace,
        Design.Domain.Enums.KnockOutPhase.Final => KnockOutPhase.Final,
        _ => throw new ArgumentOutOfRangeException(nameof(phase), $"Unsupported knock-out phase: {phase}")
    };
}
