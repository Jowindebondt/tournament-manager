
namespace TournamentManager.Domain.Test;

public static class TournamentSettingsBuilder
{
    public static T GetSingleTournamentSettings<T>()
        where T : TournamentSettings
    {
        return GetSingleTournamentSettings<T>(1);
    }
    
    public static T GetSingleTournamentSettings<T>(int id)
        where T : TournamentSettings
    {
        return GetSingleTournamentSettings<T>(id, id);
    }
    
    public static T GetSingleTournamentSettings<T>(int id, int tournamentId)
        where T : TournamentSettings
    {
        return (T)GetTournamentSettings<T>(id, tournamentId);
    }

    private static TournamentSettings GetTournamentSettings<T>(int id, int tournamentId)
        where T : TournamentSettings
    {
        return typeof(T) switch
        {
            Type tableTennisType when tableTennisType == typeof(TableTennisSettings) => new TableTennisSettings
            {
                Id = id * -1,
                TournamentId = tournamentId * -1,
                Handicap = TableTennisHandicap.None,
                TournamentType = TableTennisTournamentType.Single,
                CreatedDate = new DateTime(2024, 1, 1),
                ModifiedDate = new DateTime(2024, 1, 1),
            },
            _ => throw new ArgumentException($"Type {typeof(T)} is not supported."),
        };
    }
}
