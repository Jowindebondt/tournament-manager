using TournamentManager.Domain.Test;

namespace TournamentManager.Api.Test;

public class SimplePouleDatabaseFixture : DatabaseFixture
{
    public SimplePouleDatabaseFixture() : base(nameof(SimplePouleDatabaseFixture))
    {
    }

    protected override void CleanContext()
    {
        DbContext.Members.RemoveRange(DbContext.Members);
        DbContext.Poules.RemoveRange(DbContext.Poules);
        DbContext.Rounds.RemoveRange(DbContext.Rounds);
    }

    protected override void FillContext()
    {
        var round = RoundBuilder.GetSingleRound(1);
        DbContext.Rounds.Add(round);

        var member1 = MemberBuilder.GetSingleMember(1, 1);
        var member2 = MemberBuilder.GetSingleMember(2, 1);
        var member3 = MemberBuilder.GetSingleMember(3, 1);
        DbContext.Members.Add(member1);
        DbContext.Members.Add(member2);
        DbContext.Members.Add(member3);

        for (var i = 0; i < 10; i++)
        {
            var poule = PouleBuilder.GetSinglePoule(i + 1);
            poule.RoundId = round.Id.Value;
            DbContext.Poules.Add(poule);
        }
    }
}
