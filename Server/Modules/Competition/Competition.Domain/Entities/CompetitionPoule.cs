using CSharpFunctionalExtensions;
using Competition.Domain.ValueObjects;

namespace Competition.Domain.Entities;

public sealed class CompetitionPoule : Entity<CompetitionPouleId>
{
    public CompetitionPouleName Name { get; private set; }

    public CompetitionRoundId CompetitionRoundId { get; private set; }
    public CompetitionRound CompetitionRound { get; private set; } = null!;

    public ICollection<Competitor> Competitors { get; private set; } = [];
    public ICollection<RoundRobinGame> Games { get; private set; } = [];

    public CompetitionPoule(CompetitionPouleId id, CompetitionPouleName name, CompetitionRoundId competitionRoundId) : base(id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentNullException.ThrowIfNull(competitionRoundId, nameof(competitionRoundId));

        Name = name;
        CompetitionRoundId = competitionRoundId;
    }

    public void Rename(CompetitionPouleName newName)
    {
        ArgumentNullException.ThrowIfNull(newName, nameof(newName));

        Name = newName;
    }
}
