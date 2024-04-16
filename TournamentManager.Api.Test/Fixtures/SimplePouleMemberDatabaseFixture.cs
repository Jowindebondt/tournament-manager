using TournamentManager.Domain;
using TournamentManager.Domain.Test;

namespace TournamentManager.Api.Test;

public class SimplePouleMemberDatabaseFixture : DatabaseFixture
{
    public SimplePouleMemberDatabaseFixture() : base(nameof(SimplePouleMemberDatabaseFixture))
    {
    }

    protected override void CleanContext()
    {
        DbContext.PouleMembers.RemoveRange(DbContext.PouleMembers);
        DbContext.Members.RemoveRange(DbContext.Members);
        DbContext.Poules.RemoveRange(DbContext.Poules);
    }

    protected override void FillContext()
    {
        var poule = PouleBuilder.GetSinglePoule(1);
        DbContext.Poules.Add(poule);

        for (var i = 0; i < 10; i++)
        {
            var member = MemberBuilder.GetSingleMember(i + 1);
            DbContext.Members.Add(member);
            DbContext.PouleMembers.Add(PouleMemberBuilder.GetSinglePouleMember(i + 1, poule.Id.Value, member.Id.Value));
        }
    }
}
