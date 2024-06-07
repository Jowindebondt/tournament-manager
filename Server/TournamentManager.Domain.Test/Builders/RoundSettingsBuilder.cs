
namespace TournamentManager.Domain.Test;

public static class RoundSettingsBuilder
{
    public static RoundSettings GetSingleRoundSettings<T>()
        where T : RoundSettings
    {
        return GetSingleRoundSettings<T>(1);
    }
    
    public static RoundSettings GetSingleRoundSettings<T>(int id)
        where T : RoundSettings
    {
        return GetRoundSettings<T>(id, id);
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
