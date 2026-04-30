using Competition.Application.Interfaces;
using Competition.Domain.Entities;
using Competition.Domain.Enums;
using Competition.Domain.Interfaces;
using Competition.Domain.ValueObjects;
using CompetitionEntity = Competition.Domain.Entities.Competition;

namespace Competition.Application.Services;

/// <summary>
/// In-process implementation of <see cref="ICompetitionModuleApi"/>.
/// Creates and persists a full competition (rounds, poules, matches) from the
/// supplied creation DTO.
/// When the Competition module is extracted into a microservice, replace this
/// class with an HTTP client that implements the same interface.
/// </summary>
public class CompetitionModuleService(ICompetitionRepository competitionRepository) : ICompetitionModuleApi
{
    public async Task CreateCompetitionAsync(CompetitionCreationDto dto)
    {
        var competitionId = new CompetitionId(Guid.NewGuid());
        var name = CompetitionName.Create(dto.Name);

        if (!Enum.TryParse<Sport>(dto.Sport, out var sport))
            throw new ArgumentException($"Unsupported sport value: '{dto.Sport}'.", nameof(dto));

        var competition = new CompetitionEntity(competitionId, name, sport);

        foreach (var roundDto in dto.Rounds)
        {
            var roundId = new RoundId(Guid.NewGuid());
            var roundName = RoundName.Create(roundDto.Name);
            var round = new Round(roundId, roundName, competitionId);

            var plan = ParsePlan(roundDto.PlanType);
            if (plan != null)
                round.SetPlan(plan);

            foreach (var pouleDto in roundDto.Poules)
            {
                var pouleId = new PouleId(Guid.NewGuid());
                var pouleName = PouleName.Create(pouleDto.Name);
                var totalPlayers = PoulePlayersCount.Create((short)pouleDto.TotalPlayers);
                var poule = new Poule(pouleId, pouleName, totalPlayers, roundId);

                GenerateMatches(poule, plan);

                round.Poules.Add(poule);
            }

            competition.Rounds.Add(round);
        }

        await competitionRepository.AddAsync(competition);
    }

    private static RoundPlan? ParsePlan(string? planType) => planType switch
    {
        null => null,
        "RoundRobin" => RoundRobinPlan.Instance,
        string s when s.StartsWith("KnockOut:") =>
            new KnockOutPlan(Enum.Parse<KnockOutPhase>(s["KnockOut:".Length..])),
        _ => throw new ArgumentOutOfRangeException(nameof(planType), $"Unsupported plan type: {planType}")
    };

    private static void GenerateMatches(Poule poule, RoundPlan? plan)
    {
        var n = poule.TotalPlayers.Value;

        switch (plan)
        {
            case RoundRobinPlan:
                for (short i = 1; i <= n; i++)
                    for (short j = (short)(i + 1); j <= n; j++)
                        poule.Matches.Add(new Match(new MatchId(Guid.NewGuid()), i, j, poule.Id));
                break;

            case KnockOutPlan:
                for (short i = 1; i < n; i += 2)
                    poule.Matches.Add(new Match(new MatchId(Guid.NewGuid()), i, (short)(i + 1), poule.Id));
                break;
        }
    }
}
