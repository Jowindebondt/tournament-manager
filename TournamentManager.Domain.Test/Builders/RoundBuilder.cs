namespace TournamentManager.Domain.Test;

public class RoundBuilder
{
    public static Round GetSingleRound()
    {
        return GetSingleRound(1);
    }

    public static Round GetSingleRound(int id)
    {
        return new Round
        {
            Id = id * -1,
            Name = $"Test_Round_{id}",
            Tournament = TournamentBuilder.GetSingleTournament(id),
            CreatedDate = new DateTime(2024, 1, 1),
            ModifiedDate = new DateTime(2024, 1, 1),
        };
    }

    public static Round GetSingleRound(int id, int tournamentId)
    {
        return new Round
        {
            Id = id * -1,
            Name = $"Test_Round_{id}",
            Tournament = TournamentBuilder.GetSingleTournament(tournamentId),
            CreatedDate = new DateTime(2024, 1, 1),
            ModifiedDate = new DateTime(2024, 1, 1),
        };
    }

    public static IEnumerable<Round> GetListRound(int count)
    {
        var arr = new Round[count];
        for (var i = 0; i < count; i++)
        {
            arr[i] = GetSingleRound(i + 1);
        }

        return arr;
    }

    public static IEnumerable<Round> GetListRound(int count, int tournamentId)
    {
        var arr = new Round[count];
        for (var i = 0; i < count; i++)
        {
            arr[i] = GetSingleRound(i + 1, tournamentId);
        }

        return arr;
    }

    public static IQueryable<Round> GetListRoundAsQueryable(int count)
    {
        return GetListRound(count).AsQueryable();
    }
}
