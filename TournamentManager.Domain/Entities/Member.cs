namespace TournamentManager.Domain;

/// <summary>
/// Class defining the <see cref="Member"/> model
/// </summary>
public class Member : BaseEntity
{
    /// <summary>
    /// Name of the member
    /// </summary>
    public required string Name { get; set; }
}
