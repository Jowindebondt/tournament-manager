namespace TournamentManager.Domain;

public abstract class TournamentSettings : BaseEntity
{
    /// <summary>
    /// Identifier of the <see cref="Tournament"/> these settings are part of
    /// </summary>
    public int TournamentId { get; set; }

    /// <summary>
    /// Reference of the <see cref="Tournament"/> these settings are part of
    /// </summary>
    public virtual Tournament? Tournament { get; set; } = null!;
}
