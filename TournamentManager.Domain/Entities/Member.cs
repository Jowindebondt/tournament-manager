﻿namespace TournamentManager.Domain;

/// <summary>
/// Class defining the <see cref="Member"/> model
/// </summary>
public class Member : BaseEntity
{
    /// <summary>
    /// Reference to the related <see cref="Tournament"/>
    /// </summary>
    public Tournament Tournament { get; set; }
    /// <summary>
    /// Reference to the <see cref="Player"/>
    /// </summary>
    public Player Player { get; set; }
    /// <summary>
    /// Name of the member
    /// </summary>
    public required string Name { get; set; }
    /// <summary>
    /// The rating indicating the power of this member
    /// </summary>
    public int Rating { get; set; }
    /// <summary>
    /// The class level indicating the competition class this member is playing in
    /// </summary>
    public int Class { get; set; }
}
