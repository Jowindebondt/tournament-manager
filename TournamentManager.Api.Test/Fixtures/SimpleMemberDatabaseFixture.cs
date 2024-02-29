using TournamentManager.Domain.Test;

namespace TournamentManager.Api.Test;

public class SimpleMemberDatabaseFixture : DatabaseFixture
{
    public SimpleMemberDatabaseFixture() : base(nameof(SimpleMemberDatabaseFixture))
    {
    }

    protected override void CleanContext()
    {
        DbContext.Members.RemoveRange(DbContext.Members);
        DbContext.Tournaments.RemoveRange(DbContext.Tournaments);
    }

    protected override void FillContext()
    {
        var tournament = TournamentBuilder.GetSingleTournament(1);
        DbContext.Tournaments.Add(tournament);

        for (var i = 0; i < 10; i++)
        {
            var member = MemberBuilder.GetSingleMember(i + 1);
            member.Tournament = tournament;
            DbContext.Members.Add(member);
        }
    }
}
