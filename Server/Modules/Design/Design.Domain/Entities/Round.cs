using CSharpFunctionalExtensions;
using Design.Domain.ValueObjects;

namespace Design.Domain.Entities;

public sealed class Round : Entity<RoundId>
{
    public RoundName Name { get; private set; }
    public RoundType? Type { get; private set; }
    public RoundSettings Settings { get; private set; } = null!;
    public Round PreviousRound { get; private set; } = null!;
    public Round NextRound { get; private set; } = null!;

    public TournamentId TournamentId { get; private set; }
    public Tournament Tournament { get; private set; } = null!;

    public ICollection<Poule> Poules { get; private set; } = [];

    public Round(RoundId id, RoundName name, TournamentId tournamentId) : base(id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentNullException.ThrowIfNull(tournamentId, nameof(tournamentId));

        Name = name;
        TournamentId = tournamentId;
    }

    public void Rename(RoundName newName)
    {
        ArgumentNullException.ThrowIfNull(newName, nameof(newName));

        Name = newName;
    }

    public void SetPreviousRound(Round previousRound)
    {
        if (PreviousRound != null)
        {
            PreviousRound.NextRound = null!;
        }

        PreviousRound = previousRound;

        if (PreviousRound != null)
        {
            PreviousRound.NextRound = this;
        }
    }

    public void SetSettings(RoundSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings, nameof(settings));

        Settings = settings;
    }

    public void SetType(RoundType type)
    {
        ArgumentNullException.ThrowIfNull(type, nameof(type));

        Type = type;
    }
}
