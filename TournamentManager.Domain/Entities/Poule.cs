namespace TournamentManager.Domain;

/// <summary>
/// Class defining the <see cref="Poule"/> model 
/// </summary>
public class Poule : BaseEntity
{
    /// <summary>
    /// Reference to the related <see cref="Round"/>
    /// </summary>
    public required Round Round { get; set; }
    /// <summary>
    /// Name of the poule
    /// </summary>
    public required string Name { get; set; }
}
