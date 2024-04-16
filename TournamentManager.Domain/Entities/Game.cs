namespace TournamentManager.Domain;

/// <summary>
/// Class defining the <see cref="Game"/> model
/// </summary>
public class Game : BaseEntity
{   
    /// <summary>
    /// Reference to the related <see cref="Match"/>
    /// </summary>
    public required Match Match { get; set; }
    /// <summary>
    /// Score of <see cref="Match.Player_1"/>
    /// </summary>
    public int Score_1 { get; set; }
    /// <summary>
    /// Score of <see cref="Match.Player_2"/>
    /// </summary>
    public int Score_2 { get; set; }
}
