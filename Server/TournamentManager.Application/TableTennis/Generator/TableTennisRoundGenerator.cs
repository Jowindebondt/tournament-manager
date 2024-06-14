using TournamentManager.Domain;

namespace TournamentManager.Application;

public class TableTennisRoundGenerator : TableTennisBaseGenerator
{
    private readonly IRoundService _roundService;

    public TableTennisRoundGenerator(IRoundService roundService)
    {
        _roundService = roundService;
    }

    protected override bool CanHandle(Tournament tournament)
    {
        return tournament.Rounds.Count == 0;
    }

    protected override void Handle(Tournament tournament)
    {
        var round = new Round
        {
            Name = "Default",
            TournamentId = tournament.Id.Value,
        };
        _roundService.Insert(round);

        _roundService.SetSettings(new TableTennisRoundSettings
        {
            RoundId = round.Id.Value,
            BestOf = 5
        });
    }
}
