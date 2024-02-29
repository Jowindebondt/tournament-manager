namespace TournamentManager.Domain.Test;

public class PouleBuilder
{
    public static Poule GetSinglePoule()
    {
        return GetSinglePoule(1);
    }

    public static Poule GetSinglePoule(int id)
    {
        return new Poule
        {
            Id = id * -1,
            Name = $"Test_Poule_{id}",
            Round = RoundBuilder.GetSingleRound(id),
            CreatedDate = new DateTime(2024, 1, 1),
            ModifiedDate = new DateTime(2024, 1, 1),
        };
    }

    public static Poule GetSinglePoule(int id, int roundId)
    {
        return new Poule
        {
            Id = id * -1,
            Name = $"Test_Poule_{id}",
            Round = RoundBuilder.GetSingleRound(roundId),
            CreatedDate = new DateTime(2024, 1, 1),
            ModifiedDate = new DateTime(2024, 1, 1),
        };
    }

    public static IEnumerable<Poule> GetListPoule(int count)
    {
        var arr = new Poule[count];
        for (var i = 0; i < count; i++)
        {
            arr[i] = GetSinglePoule(i + 1);
        }

        return arr;
    }

    public static IEnumerable<Poule> GetListPoule(int count, int roundId)
    {
        var arr = new Poule[count];
        for (var i = 0; i < count; i++)
        {
            arr[i] = GetSinglePoule(i + 1, roundId);
        }

        return arr;
    }

    public static IQueryable<Poule> GetListPouleAsQueryable(int count)
    {
        return GetListPoule(count).AsQueryable();
    }
}
