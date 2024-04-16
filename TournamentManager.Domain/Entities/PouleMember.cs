namespace TournamentManager.Domain;

public class PouleMember : BaseEntity
{
    /// <summary>
    /// Reference to the related <see cref="Poule"/>
    /// </summary>
    public Poule Poule { get; set; }
    /// <summary>
    /// Reference to the related <see cref="Member"/>
    /// </summary>
    public Member Member { get; set; }
}
