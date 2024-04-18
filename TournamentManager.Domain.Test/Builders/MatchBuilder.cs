namespace TournamentManager.Domain.Test;

public class MatchBuilder
{
    public static Match GetSingleMatch()
    {
        return GetSingleMatch(1);
    }

    public static Match GetSingleMatch(int id)
    {
        return new Match
        {
            Id = id * -1,
            Player_1 = MemberBuilder.GetSingleMember(),
            Player_2 = MemberBuilder.GetSingleMember(),
            Poule = PouleBuilder.GetSinglePoule(id),
            CreatedDate = new DateTime(2024, 1, 1),
            ModifiedDate = new DateTime(2024, 1, 1),
        };
    }

    public static Match GetSingleMatch(int id, int pouleId)
    {
        return new Match
        {
            Id = id * -1,
            Player_1 = MemberBuilder.GetSingleMember(),
            Player_2 = MemberBuilder.GetSingleMember(),
            Poule = PouleBuilder.GetSinglePoule(pouleId),
            CreatedDate = new DateTime(2024, 1, 1),
            ModifiedDate = new DateTime(2024, 1, 1),
        };
    }
    
    public static IEnumerable<Match> GetListMatch(int count, int pouleId)
    {
        var arr = new Match[count];
        for (var i = 0; i < count; i++)
        {
            arr[i] = GetSingleMatch(i + 1, pouleId);
        }

        return arr;
    }
}
