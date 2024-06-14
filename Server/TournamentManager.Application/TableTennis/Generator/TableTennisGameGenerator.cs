using TournamentManager.Domain;

namespace TournamentManager.Application;

public class TableTennisGameGenerator : TableTennisBaseGenerator
{
    private readonly IGameService _gameService;

    public TableTennisGameGenerator(IGameService gameService)
    {
        _gameService = gameService;
    }

    protected override bool CanHandle(Tournament tournament)
    {
        return tournament.Rounds.Any(u => u.Poules.Any(v => v.Matches.Any(x => x.Games.Count == 0)));
    }

    protected override void Handle(Tournament tournament)
    {
        var matches = tournament.Rounds.SelectMany(u => u.Poules.SelectMany(v => v.Matches));
        foreach (var match in matches)
        {
            if (match.Games.Count > 0)
            {
                continue;
            }

            var roundSettings = match.Poule.Round.Settings as TableTennisRoundSettings;
            for (var i = 0; i < roundSettings.BestOf; i++)
            {
                _gameService.Insert(new Game{ 
                    MatchId = match.Id.Value
                });
            }
        }
    }
}
