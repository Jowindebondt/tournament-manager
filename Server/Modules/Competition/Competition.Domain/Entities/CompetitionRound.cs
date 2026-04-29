using CSharpFunctionalExtensions;
using Competition.Domain.ValueObjects;

namespace Competition.Domain.Entities;

public sealed class CompetitionRound : Entity<CompetitionRoundId>
{
    public CompetitionRoundName Name { get; private set; }
    public CompetitionPlan Plan { get; private set; } = null!;

    public CompetitionId CompetitionId { get; private set; }
    public Competition Competition { get; private set; } = null!;

    public ICollection<CompetitionPoule> Poules { get; private set; } = [];

    public CompetitionRound(CompetitionRoundId id, CompetitionRoundName name, CompetitionId competitionId) : base(id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentNullException.ThrowIfNull(competitionId, nameof(competitionId));

        Name = name;
        CompetitionId = competitionId;
    }

    public void Rename(CompetitionRoundName newName)
    {
        ArgumentNullException.ThrowIfNull(newName, nameof(newName));

        Name = newName;
    }

    public void SetPlan(CompetitionPlan plan)
    {
        ArgumentNullException.ThrowIfNull(plan, nameof(plan));

        Plan = plan;
    }
}
