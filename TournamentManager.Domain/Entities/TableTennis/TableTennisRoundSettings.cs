namespace TournamentManager.Domain;

public class TableTennisRoundSettings : RoundSettings
{
    /// <summary>
    /// Indication of the minimum amount of games that must be played to determine the winner
    /// </summary>
    public int BestOf { get; set; }
}
