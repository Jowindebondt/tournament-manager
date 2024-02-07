namespace TournamentManager.Domain.Test;

public static class TournamentBuilder
{
    public static Tournament GetSingleTournament()
    {
        return GetSingleTournament(1);
    }

    public static Tournament GetSingleTournament(int id)
    {
        return new Tournament
        {
            Id = id * -1,
            Name = $"Test_Tournament_{id}",
            CreatedDate = new DateTime(2024, 1, 1),
            ModifiedDate = new DateTime(2024, 1, 1),
        };
    }

    public static IEnumerable<Tournament> GetListTournament(int count)
    {
        var arr = new Tournament[count];
        for (var i = 0; i < count; i++)
        {
            arr[i] = GetSingleTournament(i + 1);
        }

        return arr;
    }

    public static IQueryable<Tournament> GetListTournamentAsQueryable(int count)
    {
        return GetListTournament(count).AsQueryable();
    }
}
