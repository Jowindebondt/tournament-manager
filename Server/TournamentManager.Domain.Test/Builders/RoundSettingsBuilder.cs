
namespace TournamentManager.Domain.Test;

public static class RoundSettingsBuilder
{
    public static T GetSingleRoundSettings<T>()
        where T : RoundSettings
    {
        return GetSingleRoundSettings<T>(1);
    }
    
    public static T GetSingleRoundSettings<T>(int id)
        where T : RoundSettings
    {
        return (T)GetSingleRoundSettings<T>(id, id);
    }
    
    public static T GetSingleRoundSettings<T>(int id, int roundId)
        where T : RoundSettings
    {
        return (T)GetRoundSettings<T>(id, roundId);
    }

    private static RoundSettings GetRoundSettings<T>(int id, int roundId)
        where T : RoundSettings
    {
        return typeof(T) switch
        {
            Type tableTennisType when tableTennisType == typeof(TableTennisRoundSettings) => new TableTennisRoundSettings
            {
                Id = id,
                RoundId = roundId,
                BestOf = 5,
                CreatedDate = new DateTime(2024, 1, 1),
                ModifiedDate = new DateTime(2024, 1, 1),
            },
            _ => throw new ArgumentException($"Type {typeof(T)} is not supported."),
        };
    }
}
