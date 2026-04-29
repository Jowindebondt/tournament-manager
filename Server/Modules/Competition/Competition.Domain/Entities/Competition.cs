using CSharpFunctionalExtensions;
using Competition.Domain.Enums;
using Competition.Domain.ValueObjects;

namespace Competition.Domain.Entities;

public sealed class Competition : Entity<CompetitionId>
{
    public CompetitionName Name { get; private set; }
    public Sport Sport { get; private set; }
    public CompetitionStatus Status { get; private set; }

    public ICollection<CompetitionRound> Rounds { get; private set; } = [];

    public Competition(CompetitionId id, CompetitionName name, Sport sport) : base(id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        ArgumentNullException.ThrowIfNull(name, nameof(name));

        Name = name;
        Sport = sport;
        Status = CompetitionStatus.Planned;
    }

    public void Rename(CompetitionName newName)
    {
        ArgumentNullException.ThrowIfNull(newName, nameof(newName));

        Name = newName;
    }

    public void Start()
    {
        if (Status != CompetitionStatus.Planned)
        {
            throw new InvalidOperationException("Competition can only be started when in Planned status.");
        }

        Status = CompetitionStatus.Active;
    }

    public void Complete()
    {
        if (Status != CompetitionStatus.Active)
        {
            throw new InvalidOperationException("Competition can only be completed when in Active status.");
        }

        Status = CompetitionStatus.Completed;
    }
}
