namespace TournamentManager.Domain;

public class PoulePlayer : BaseEntity
{
    /// <summary>
    /// Reference to the related <see cref="Poule"/>
    /// </summary>
    public Poule Poule { get; set; }
    /// <summary>
    /// Reference to the related <see cref="Player"/>
    /// </summary>
    public Player Player { get; set; }
}
