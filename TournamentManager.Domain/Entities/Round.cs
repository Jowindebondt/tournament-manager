﻿namespace TournamentManager.Domain;

/// <summary>
/// Class defining the <see cref="Round"/> model
/// </summary>
public class Round : BaseEntity
{
    /// <summary>
    /// Reference to the related <see cref="Tournament"/>
    /// </summary>
    public Tournament Tournament { get; set; }
    /// <summary>
    /// Name of the round
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Reference of the <see cref="RoundSettings"/> that are applied to the round
    /// </summary>
    public virtual RoundSettings Settings { get; set; } = null!;
}
