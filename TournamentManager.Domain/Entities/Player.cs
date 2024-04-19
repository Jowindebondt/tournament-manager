namespace TournamentManager.Domain;

public class Player : BaseEntity
{
    /// <summary>
    /// Collection of <see cref="Poule"/>s this player is part of
    /// </summary>
    public List<Poule> Poules { get; set; } = [];
}
