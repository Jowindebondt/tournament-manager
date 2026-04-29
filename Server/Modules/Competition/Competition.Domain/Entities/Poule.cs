using CSharpFunctionalExtensions;
using Competition.Domain.ValueObjects;

namespace Competition.Domain.Entities;

public sealed class Poule : Entity<PouleId>
{
    public PouleName Name { get; private set; }
    public PoulePlayersCount TotalPlayers { get; private set; }

    public RoundId RoundId { get; private set; }
    public Round Round { get; private set; } = null!;

    public ICollection<Match> Matches { get; private set; } = [];

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
}
