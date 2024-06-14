using TournamentManager.Domain;

namespace TournamentManager.Application;

public class Poule4PlayerTemplateService : BasePouleTemplateService
{
    public override Poule GetTemplate()
    {
        var players = GetPlayers(4);

        return new Poule {
            Name = "Default",
            Players = players.ToList(),
            Matches = [
                GetMatch(players, 1, 4),
                GetMatch(players, 2, 3),
                GetMatch(players, 1, 3),
                GetMatch(players, 2, 4),
                GetMatch(players, 1, 2),
                GetMatch(players, 3, 4),
            ]
        };
    }
}
