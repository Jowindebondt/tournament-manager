using CSharpFunctionalExtensions;
using Competition.Domain.ValueObjects;

namespace Competition.Domain.Entities;

public sealed class Round : Entity<RoundId>
{
    public RoundName Name { get; private set; }
    public RoundPlan? Plan { get; private set; }

    public CompetitionId CompetitionId { get; private set; }
    public Competition Competition { get; private set; } = null!;

    public ICollection<Poule> Poules { get; private set; } = [];

    public Round(RoundId id, RoundName name, CompetitionId competitionId) : base(id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentNullException.ThrowIfNull(competitionId, nameof(competitionId));

        Name = name;
        CompetitionId = competitionId;
    }

    public void SetPlan(RoundPlan plan)
    {
        ArgumentNullException.ThrowIfNull(plan, nameof(plan));

        Plan = plan;
    }
}
