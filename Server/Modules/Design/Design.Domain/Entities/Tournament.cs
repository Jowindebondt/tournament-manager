using CSharpFunctionalExtensions;
using Design.Domain.Enums;
using Design.Domain.ValueObjects;

namespace Design.Domain.Entities;

public sealed class Tournament : Entity<TournamentId>
{
    public TournamentName Name { get; private set; }
    public Sport Sport { get; private set; }

    public ICollection<Round> Rounds { get; private set; } = [];

    public Tournament(TournamentId id, TournamentName name, Sport sport) : base(id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        ArgumentNullException.ThrowIfNull(name, nameof(name));

        Name = name;
        Sport = sport;
    }

    public void Rename(TournamentName newName)
    {
        ArgumentNullException.ThrowIfNull(newName, nameof(newName));

        Name = newName;
    }
}
