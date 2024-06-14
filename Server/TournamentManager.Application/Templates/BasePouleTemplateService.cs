using TournamentManager.Domain;

namespace TournamentManager.Application;

public abstract class BasePouleTemplateService : IPouleTemplateService
{
    public abstract Poule GetTemplate();

    protected IEnumerable<Player> GetPlayers(int amountPlayers)
    {
        var players = new List<Player>();

        for (var i = 0; i < amountPlayers; i++)
        {
            players.Add(new Player());
        }
        
        return players;
    }

    protected Match GetMatch(IEnumerable<Player> players, int player1, int player2)
    {
        return new Match
        {
            Player1 = players.ElementAt(player1 - 1),
            Player2 = players.ElementAt(player2 - 1),
        };
    }
}
