namespace TournamentManager.Domain;

public class TableTennisRoundSettings : BaseEntity
{
    public required Round Round { get; set; }
    public int BestOf { get; set; }
}
