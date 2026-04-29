using System.Diagnostics;
using CSharpFunctionalExtensions;
using Competition.Domain.ValueObjects;

namespace Competition.Domain.Entities;

/// <summary>
/// Represents a single game in a knockout bracket belonging to a CompetitionRound.
/// Competitors may be null when they are not yet determined (e.g., awaiting a previous round result).
/// </summary>
[DebuggerDisplay($"Home={{{nameof(HomeCompetitorId)}}}, Away={{{nameof(AwayCompetitorId)}}}")]
public sealed class BracketGame : Entity<BracketGameId>
{
    public CompetitorId? HomeCompetitorId { get; private set; }
    public Competitor? HomeCompetitor { get; private set; }

    public CompetitorId? AwayCompetitorId { get; private set; }
    public Competitor? AwayCompetitor { get; private set; }

    public CompetitionRoundId CompetitionRoundId { get; private set; }
    public CompetitionRound CompetitionRound { get; private set; } = null!;

    public int? HomeScore { get; private set; }
    public int? AwayScore { get; private set; }

    public BracketGame(BracketGameId id, CompetitionRoundId competitionRoundId) : base(id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        ArgumentNullException.ThrowIfNull(competitionRoundId, nameof(competitionRoundId));

        CompetitionRoundId = competitionRoundId;
    }

    public void SetCompetitors(CompetitorId homeCompetitorId, CompetitorId awayCompetitorId)
    {
        ArgumentNullException.ThrowIfNull(homeCompetitorId, nameof(homeCompetitorId));
        ArgumentNullException.ThrowIfNull(awayCompetitorId, nameof(awayCompetitorId));

        HomeCompetitorId = homeCompetitorId;
        AwayCompetitorId = awayCompetitorId;
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
