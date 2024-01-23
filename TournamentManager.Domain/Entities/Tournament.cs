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
}
