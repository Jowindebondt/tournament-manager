namespace TournamentManager.Domain.Test;

public class GameBuilder
{
    public static Game GetSingleGame()
    {
        return GetSingleGame(1);
    }

    public static Game GetSingleGame(int id)
    {
        return GetSingleGame(id,id);
    }

    public static Game GetSingleGame(int id, int matchId)
    {
        return new Game
        {
            Id = id * -1,
            Score_1 = 0,
            Score_2 = 0,
            MatchId = matchId * -1,
            CreatedDate = new DateTime(2024, 1, 1),
            ModifiedDate = new DateTime(2024, 1, 1),
        };
    }

    public static IEnumerable<Game> GetListGame(int count)
    {
        var arr = new Game[count];
        for (var i = 0; i < count; i++)
        {
            arr[i] = GetSingleGame(i + 1);
        }

        return arr;
    }

    public static IEnumerable<Game> GetListGame(int count, int roundId)
    {
        var arr = new Game[count];
        for (var i = 0; i < count; i++)
        {
            arr[i] = GetSingleGame(i + 1, roundId);
        }

        return arr;
    }
}
