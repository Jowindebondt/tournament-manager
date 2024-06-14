using TournamentManager.Domain;

namespace TournamentManager.Application;

public class Poule5PlayerTemplateService : BasePouleTemplateService
{
    public override Poule GetTemplate()
    {
        var players = GetPlayers(5);

        return new Poule {
            Name = "Default",
            Players = players.ToList(),
            Matches = [
                GetMatch(players, 2, 5),
                GetMatch(players, 3, 4),
                GetMatch(players, 1, 5),
                GetMatch(players, 2, 4),
                GetMatch(players, 1, 4),
                GetMatch(players, 2, 3),
                GetMatch(players, 1, 3),
                GetMatch(players, 4, 5),
                GetMatch(players, 1, 2),
                GetMatch(players, 3, 5),
            ]
        };
    }
}
