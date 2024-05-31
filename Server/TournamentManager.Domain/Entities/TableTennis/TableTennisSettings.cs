namespace TournamentManager.Domain;

public class TableTennisSettings : TournamentSettings
{
    public TableTennisHandicap Handicap { get; set; }
    public TableTennisTournamentType TournamentType { get; set; }
}
