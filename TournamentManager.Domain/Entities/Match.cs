namespace TournamentManager.Domain;

/// <summary>
/// Class defining the <see cref="Match"/> model
/// </summary>
public class Match : BaseEntity
{
    /// <summary>
    /// Reference to the related <see cref="Poule"/>
    /// </summary>
    public Poule Poule { get; set; }
    /// <summary>
    /// Player 1 reference to <see cref="Player"/>
    /// </summary>
    public required Player Player_1 { get; set; }
    /// <summary>
    /// Player 2 reference to <see cref="Player"/>
    /// </summary>
    public required Player Player_2 { get; set; }
}
