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
        DbContext.Members.RemoveRange(DbContext.Members);
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

        var member1 = MemberBuilder.GetSingleMember(1);
        var member2 = MemberBuilder.GetSingleMember(2);
        member1.Tournament = tournament;
        member2.Tournament = tournament;
        DbContext.Members.Add(member1);
        DbContext.Members.Add(member2);

        for (var i = 0; i< 10; i++)
        {
            var match = MatchBuilder.GetSingleMatch(i + 1);
            match.Poule = poule;
            match.Player_1 = member1;
            match.Player_2 = member2;
            DbContext.Matches.Add(match);
        }
    }
}
