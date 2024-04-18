namespace TournamentManager.Domain;

public class TableTennisSettings : BaseEntity
{
    public required Tournament Tournament { get; set; }
    public TableTennisHandicap Handicap { get; set; }
    public TableTennisTournamentType TournamentType { get; set; }
}
