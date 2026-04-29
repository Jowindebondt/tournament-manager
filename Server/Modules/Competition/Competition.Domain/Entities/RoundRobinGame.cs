using System.Diagnostics;
using CSharpFunctionalExtensions;
using Competition.Domain.ValueObjects;

namespace Competition.Domain.Entities;

/// <summary>
/// Represents a single game between two competitors in a round-robin poule.
/// </summary>
[DebuggerDisplay($"Home={{{nameof(HomeCompetitorId)}}}, Away={{{nameof(AwayCompetitorId)}}}")]
public sealed class RoundRobinGame : Entity<RoundRobinGameId>
{
    public CompetitorId HomeCompetitorId { get; private set; }
    public Competitor HomeCompetitor { get; private set; } = null!;

    public CompetitorId AwayCompetitorId { get; private set; }
    public Competitor AwayCompetitor { get; private set; } = null!;

    public CompetitionPouleId CompetitionPouleId { get; private set; }
    public CompetitionPoule CompetitionPoule { get; private set; } = null!;

    public int? HomeScore { get; private set; }
    public int? AwayScore { get; private set; }

    public RoundRobinGame(
        RoundRobinGameId id,
        CompetitorId homeCompetitorId,
        CompetitorId awayCompetitorId,
        CompetitionPouleId competitionPouleId) : base(id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        ArgumentNullException.ThrowIfNull(homeCompetitorId, nameof(homeCompetitorId));
        ArgumentNullException.ThrowIfNull(awayCompetitorId, nameof(awayCompetitorId));
        ArgumentNullException.ThrowIfNull(competitionPouleId, nameof(competitionPouleId));

        HomeCompetitorId = homeCompetitorId;
        AwayCompetitorId = awayCompetitorId;
        CompetitionPouleId = competitionPouleId;
    }

    public void SetScore(int homeScore, int awayScore)
    {
        if (homeScore < 0) throw new ArgumentOutOfRangeException(nameof(homeScore), "Score cannot be negative.");
        if (awayScore < 0) throw new ArgumentOutOfRangeException(nameof(awayScore), "Score cannot be negative.");

        HomeScore = homeScore;
        AwayScore = awayScore;
    }

    public void ClearScore()
    {
        HomeScore = null;
        AwayScore = null;
    }
}
