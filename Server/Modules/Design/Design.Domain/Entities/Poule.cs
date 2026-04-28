using CSharpFunctionalExtensions;
using Design.Domain.ValueObjects;

namespace Design.Domain.Entities;

public sealed class Poule : Entity<PouleId>
{
    public PouleName Name { get; private set; }
    public PoulePlayersCount TotalPlayers { get; private set; }

    public RoundId RoundId { get; private set; }
    public Round Round { get; private set; } = null!;

    public Poule(PouleId id, PouleName name, PoulePlayersCount totalPlayers, RoundId roundId) : base(id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentNullException.ThrowIfNull(totalPlayers, nameof(totalPlayers));
        ArgumentNullException.ThrowIfNull(roundId, nameof(roundId));

        Name = name;
        TotalPlayers = totalPlayers;
        RoundId = roundId;
    }

    public void Rename(PouleName newName)
    {
        ArgumentNullException.ThrowIfNull(newName, nameof(newName));

        Name = newName;
    }

    public void SetTotalPlayers(PoulePlayersCount totalPlayers)
    {
        ArgumentNullException.ThrowIfNull(totalPlayers, nameof(totalPlayers));

        TotalPlayers = totalPlayers;
    }
}
