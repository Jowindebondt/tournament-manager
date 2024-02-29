namespace TournamentManager.Domain.Test;

public class MemberBuilder
{
    public static Member GetSingleMember()
    {
        return GetSingleMember(1);
    }

    public static Member GetSingleMember(int id)
    {
        return new Member
        {
            Id = id * -1,
            Name = $"Test_Member_{id}",
            Tournament = TournamentBuilder.GetSingleTournament(id),
            CreatedDate = new DateTime(2024, 1, 1),
            ModifiedDate = new DateTime(2024, 1, 1),
        };
    }

    public static Member GetSingleMember(int id, int tournamentId)
    {
        return new Member
        {
            Id = id * -1,
            Name = $"Test_Member_{id}",
            Tournament = TournamentBuilder.GetSingleTournament(tournamentId),
            CreatedDate = new DateTime(2024, 1, 1),
            ModifiedDate = new DateTime(2024, 1, 1),
        };
    }

    public static IEnumerable<Member> GetListMember(int count)
    {
        var arr = new Member[count];
        for (var i = 0; i < count; i++)
        {
            arr[i] = GetSingleMember(i + 1);
        }

        return arr;
    }

    public static IEnumerable<Member> GetListMember(int count, int tournamentId)
    {
        var arr = new Member[count];
        for (var i = 0; i < count; i++)
        {
            arr[i] = GetSingleMember(i + 1, tournamentId);
        }

        return arr;
    }

    public static IQueryable<Member> GetListMemberAsQueryable(int count)
    {
        return GetListMember(count).AsQueryable();
    }
}
