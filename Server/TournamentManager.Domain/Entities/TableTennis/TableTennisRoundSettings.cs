namespace TournamentManager.Domain;

public class TableTennisRoundSettings : RoundSettings
{
    /// <summary>
    /// Indication of the maximum amount of games that must be played to determine the winner. The minimum amount of will be calculated by dividing by 2 and rounding the result up. For example if this property is set to '5' the minimum amount will be '3'.
    /// </summary>
    public int BestOf { get; set; }
}
