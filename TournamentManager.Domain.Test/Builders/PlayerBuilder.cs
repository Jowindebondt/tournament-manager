
namespace TournamentManager.Domain.Test;

public class PlayerBuilder
{
    public static Player GetSinglePlayer()
    {
        return GetSinglePlayer(1);
    }

    public static Player GetSinglePlayer(int id)
    {
        return new Player
        {
            Id = id * -1,
            CreatedDate = new DateTime(2024, 1, 1),
            ModifiedDate = new DateTime(2024, 1, 1),
        };
    }

    public static Player GetSinglePlayer(int id, int tournamentId)
    {
        return new Player
        {
            Id = id * -1,
            CreatedDate = new DateTime(2024, 1, 1),
            ModifiedDate = new DateTime(2024, 1, 1),
        };
    }

    public static IEnumerable<Player> GetListPlayer(int count)
    {
        var arr = new Player[count];
        for (var i = 0; i < count; i++)
        {
            arr[i] = GetSinglePlayer(i + 1);
        }

        return arr;
    }

    public static IEnumerable<Player> GetListPlayer(int count, int tournamentId)
    {
        var arr = new Player[count];
        for (var i = 0; i < count; i++)
        {
            arr[i] = GetSinglePlayer(i + 1, tournamentId);
        }

        return arr;
    }

    public static IQueryable<Player> GetListPlayerAsQueryable(int count)
    {
        return GetListPlayer(count).AsQueryable();
    }
}
