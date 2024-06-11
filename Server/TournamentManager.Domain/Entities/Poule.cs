namespace TournamentManager.Domain;

/// <summary>
/// Class defining the <see cref="Poule"/> model 
/// </summary>
public class Poule : BaseEntity
{
    /// <summary>
    /// Identifier of the related <see cref="Round"/>
    /// </summary>
    public int RoundId { get; set; }

    /// <summary>
    /// Name of the poule
    /// </summary>
    public required string Name { get; set; }
    /// <summary>
    /// Collection of assigned <see cref="Player"/>s to this poule
    /// </summary>
    public virtual List<Player> Players { get; set; } = [];

    /// <summary>
    /// Reference to the related <see cref="Round"/>
    /// </summary>
    public virtual Round? Round { get; set; } = null!;
    
    /// <summary>
    /// Collection of <see cref="Match"/>es that are part of this poule
    /// </summary>
    public virtual ICollection<Match> Matches { get; set; } = [];
}
