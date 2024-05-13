using TournamentManager.Domain.Test;

namespace TournamentManager.Api.Test;

public class SimpleRoundDatabaseFixture : DatabaseFixture
{
    public SimpleRoundDatabaseFixture() : base(nameof(SimpleRoundDatabaseFixture))
    {
    }

    protected override void CleanContext()
    {
        DbContext.Rounds.RemoveRange(DbContext.Rounds);
        DbContext.Tournaments.RemoveRange(DbContext.Tournaments);
    }

    protected override void FillContext()
    {
        var tournament = TournamentBuilder.GetSingleTournament(1);
        DbContext.Tournaments.Add(tournament);

        for (var i = 0; i < 10; i++)
        {
            var round = RoundBuilder.GetSingleRound(i + 1);
            round.TournamentId = tournament.Id.Value;
            DbContext.Rounds.Add(round);
        }
    }
}
