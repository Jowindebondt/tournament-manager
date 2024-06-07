namespace TournamentManager.Domain;

public abstract class RoundSettings : BaseEntity 
{
    /// <summary>
    /// Identifier of the <see cref="Round"/> these settings are part of
    /// </summary>
    public int RoundId { get; set; }

    /// <summary>
    /// Reference of the <see cref="Round"/> these settings are part of
    /// </summary>
    public virtual Round? Round { get; set; } = null!;
}
