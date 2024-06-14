using TournamentManager.Domain;

namespace TournamentManager.Application;

public class Poule3PlayerTemplateService : BasePouleTemplateService
{
    public override Poule GetTemplate()
    {
        var players = GetPlayers(3);

        return new Poule {
            Name = "Default",
            Players = players.ToList(),
            Matches = [
                GetMatch(players, 2, 3),
                GetMatch(players, 1, 3),
                GetMatch(players, 1, 2),
            ]
        };
    }
}
