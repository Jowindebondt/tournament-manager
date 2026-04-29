using CSharpFunctionalExtensions;
using Competition.Domain.Enums;
using Competition.Domain.ValueObjects;

namespace Competition.Domain.Entities;

public sealed class Competition : Entity<CompetitionId>
{
    public CompetitionName Name { get; private set; }
    public Sport Sport { get; private set; }

    public ICollection<Round> Rounds { get; private set; } = [];

    public Competition(CompetitionId id, CompetitionName name, Sport sport) : base(id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        ArgumentNullException.ThrowIfNull(name, nameof(name));

        Name = name;
        Sport = sport;
    }
}
