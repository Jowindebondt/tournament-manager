namespace TournamentManager.Domain;

/// <summary>
/// Class defining the <see cref="Match"/> model
/// </summary>
public class Match : BaseEntity
{
    /// <summary>
    /// Identifier of the related <see cref="Poule"/>
    /// </summary>
    public int PouleId { get; set; }
    /// <summary>
    /// Identifier of <see cref="Player"/> 1
    /// </summary>
    public int Player1Id { get; set; }
    /// <summary>
    /// Identifier of <see cref="Player"/> 2
    /// </summary>
    public int Player2Id { get; set; }

    /// <summary>
    /// Reference to the related <see cref="Poule"/>
    /// </summary>
    public virtual Poule Poule { get; set; } = null!;
    /// <summary>
    /// Player 1 reference to <see cref="Player"/>
    /// </summary>
    public virtual Player Player1 { get; set; } = null!;
    /// <summary>
    /// Player 2 reference to <see cref="Player"/>
    /// </summary>
    public virtual Player Player2 { get; set; } = null!;

    /// <summary>
    /// Collection of games which makes the match
    /// </summary>
    public virtual ICollection<Game> Games { get; set; } = [];
}
