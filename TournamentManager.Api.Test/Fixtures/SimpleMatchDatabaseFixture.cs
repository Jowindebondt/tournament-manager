using TournamentManager.Domain.Test;

namespace TournamentManager.Api.Test;

public class SimpleMatchDatabaseFixture : DatabaseFixture
{
    public SimpleMatchDatabaseFixture(): base(nameof(SimpleMatchDatabaseFixture))
    {

    }
    
    protected override void CleanContext()
    {
        DbContext.Matches.RemoveRange(DbContext.Matches);
        DbContext.Players.RemoveRange(DbContext.Players);
        DbContext.Poules.RemoveRange(DbContext.Poules);
        DbContext.Rounds.RemoveRange(DbContext.Rounds);
        DbContext.Tournaments.RemoveRange(DbContext.Tournaments);
    }

    protected override void FillContext()
    {
        var tournament = TournamentBuilder.GetSingleTournament(1);
        DbContext.Tournaments.Add(tournament);

        var round = RoundBuilder.GetSingleRound(1);
        round.Tournament = tournament;
        DbContext.Rounds.Add(round);

        var poule = PouleBuilder.GetSinglePoule(1);
        poule.Round = round;
        DbContext.Poules.Add(poule);

        var player1 = PlayerBuilder.GetSinglePlayer(1);
        var player2 = PlayerBuilder.GetSinglePlayer(2);
        DbContext.Players.Add(player1);
        DbContext.Players.Add(player2);

        for (var i = 0; i< 10; i++)
        {
            var match = MatchBuilder.GetSingleMatch(i + 1);
            match.PouleId = poule.Id.Value;
            match.Player1Id = player1.Id.Value;
            match.Player2Id = player2.Id.Value;
            DbContext.Matches.Add(match);
        }
    }
}
