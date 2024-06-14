using System.Data;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;

namespace TournamentManager.Api.Test;

public class SimpleTournamentDatabaseFixture : DatabaseFixture
{
    public SimpleTournamentDatabaseFixture() : base(nameof(SimpleTournamentDatabaseFixture))
    {
    }

    protected override void CleanContext()
    {
        DbContext.Games.RemoveRange(DbContext.Games);
        DbContext.Matches.RemoveRange(DbContext.Matches);
        DbContext.Players.RemoveRange(DbContext.Players);
        DbContext.Members.RemoveRange(DbContext.Members);
        DbContext.Poules.RemoveRange(DbContext.Poules);
        DbContext.Rounds.RemoveRange(DbContext.Rounds);
        DbContext.Tournaments.RemoveRange(DbContext.Tournaments);
    }

    protected override void FillContext()
    {
        var memberId = 1;
        for (var i = 0; i < 10; i++)
        {
            var tournament = TournamentBuilder.GetSingleTournament(i + 1);
            tournament.Settings = new TableTennisSettings
            {
                TournamentType = TableTennisTournamentType.Single
            };
            DbContext.Tournaments.Add(tournament);
            
            for (var j = 0; j < 3; j++)
            {
                var member = MemberBuilder.GetSingleMember(memberId, i + 1);
                memberId++;
                DbContext.Members.Add(member);
            }
        }
    }
}
