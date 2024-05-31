using TournamentManager.Domain.Test;

namespace TournamentManager.Api.Test;

public class SimpleTournamentDatabaseFixture : DatabaseFixture
{
    public SimpleTournamentDatabaseFixture() : base(nameof(SimpleTournamentDatabaseFixture))
    {
    }

    protected override void CleanContext()
    {
        DbContext.Tournaments.RemoveRange(DbContext.Tournaments);
    }

    protected override void FillContext()
    {
        for (var i = 0; i < 10; i++)
        {
            var tournament = TournamentBuilder.GetSingleTournament(i + 1);
            DbContext.Tournaments.Add(tournament);
        }
    }
}
