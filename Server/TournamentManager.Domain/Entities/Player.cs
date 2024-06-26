﻿namespace TournamentManager.Domain;

public class Player : BaseEntity
{
    /// <summary>
    /// Collection of <see cref="Poule"/>s this player is part of
    /// </summary>
    public virtual List<Poule> Poules { get; set; } = [];

    /// <summary>
    /// Collection of <see cref="Match"/>es that this player is assigned to as player 1
    /// </summary>
    public virtual ICollection<Match> MatchesAsPlayer1 { get; set; } = [];
    /// <summary>
    /// Collection of <see cref="Match"/>es that this player is assigned to as player 2
    /// </summary>
    public virtual ICollection<Match> MatchesAsPlayer2 { get; set; } = [];
    /// <summary>
    /// Collection of <see cref="Member"/>s that are part of this player
    /// </summary>
    public virtual ICollection<Member> Members { get; set; } = [];
}
