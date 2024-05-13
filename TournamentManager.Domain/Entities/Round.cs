namespace TournamentManager.Domain;

/// <summary>
/// Class defining the <see cref="Round"/> model
/// </summary>
public class Round : BaseEntity
{
    /// <summary>
    /// Identifier of the related <see cref="Tournament"/>
    /// </summary>
    public int TournamentId { get; set; }
    /// <summary>
    /// Name of the round
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Reference to the related <see cref="Tournament"/>
    /// </summary>
    public virtual Tournament Tournament { get; set; } = null!;
    
    /// <summary>
    /// Reference of the <see cref="RoundSettings"/> that are applied to the round
    /// </summary>
    public virtual RoundSettings Settings { get; set; } = null!;
    /// <summary>
    /// Collection of <see cref="Poule"/>s that are part of this round.
    /// </summary>
    public virtual ICollection<Poule> Poules { get; set; } = [];
}
