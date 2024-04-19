namespace TournamentManager.Domain;

/// <summary>
/// Class defining the <see cref="Poule"/> model 
/// </summary>
public class Poule : BaseEntity
{
    /// <summary>
    /// Reference to the related <see cref="Round"/>
    /// </summary>
    public Round Round { get; set; }
    /// <summary>
    /// Name of the poule
    /// </summary>
    public required string Name { get; set; }
    /// <summary>
    /// Collection of assigned <see cref="Player"/>s to this poule
    /// </summary>
    public List<Player> Players { get; set; } = [];

    /// <summary>
    /// Collection of <see cref="Match"/>es that are part of this poule
    /// </summary>
    public virtual ICollection<Match> Matches { get; set; } = [];
}
