using TournamentManager.Domain.Test;

namespace TournamentManager.Api.Test;

public class SimplePouleDatabaseFixture : DatabaseFixture
{
    public SimplePouleDatabaseFixture() : base(nameof(SimplePouleDatabaseFixture))
    {
    }

    protected override void CleanContext()
    {
        DbContext.Poules.RemoveRange(DbContext.Poules);
        DbContext.Rounds.RemoveRange(DbContext.Rounds);
    }

    protected override void FillContext()
    {
        var round = RoundBuilder.GetSingleRound(1);
        DbContext.Rounds.Add(round);

        for (var i = 0; i < 10; i++)
        {
            var poule = PouleBuilder.GetSinglePoule(i + 1);
            poule.Round = round;
            DbContext.Poules.Add(poule);
        }
    }
}
