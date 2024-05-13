namespace TournamentManager.Domain;

/// <summary>
/// Class defining the <see cref="Tournament"/> model
/// </summary>
public class Tournament : BaseEntity
{
    /// <summary>
    /// Name of the tournament
    /// </summary>
    public required string Name { get; set; }
    /// <summary>
    /// Type of sport
    /// </summary>
    public Sport Sport { get; set; }
    
    /// <summary>
    /// Reference of the <see cref="TournamentSettings"/> that are applied to the tournament
    /// </summary>
    public virtual TournamentSettings Settings { get; set; } = null!;

    /// <summary>
    /// Collection of all the members that are participating in this tournament
    /// </summary>
    public virtual ICollection<Member> Members { get; set; } = [];
}
