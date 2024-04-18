using TournamentManager.Domain.Test;

namespace TournamentManager.Api.Test;

public class SimpleGameDatabaseFixture : DatabaseFixture
{
    public SimpleGameDatabaseFixture() : base(nameof(SimpleGameDatabaseFixture))
    {
    }

    protected override void CleanContext()
    {
        DbContext.Games.RemoveRange(DbContext.Games);
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

        var match = MatchBuilder.GetSingleMatch(1);
        match.Poule = poule;
        match.Player_1 = player1;
        match.Player_2 = player2;
        DbContext.Matches.Add(match);

        for (var i = 0; i < 10; i++)
        {
            var game = GameBuilder.GetSingleGame(i + 1);
            game.Match = match;
            DbContext.Games.Add(game);
        }
    }
}
