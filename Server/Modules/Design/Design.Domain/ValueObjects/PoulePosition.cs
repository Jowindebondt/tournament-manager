using System.Diagnostics;
using CSharpFunctionalExtensions;
using Design.Domain.Entities;

namespace Design.Domain.ValueObjects;

[DebuggerDisplay($"{nameof(Poule)} = {{{nameof(Poule)}}}, {nameof(Position)} = {{{nameof(Position)}}}")]
public sealed class PoulePosition : ValueObject
{
    public Poule Poule { get; }
    public short Position { get; }

    private PoulePosition(Poule poule, short position)
    {
        Poule = poule;
        Position = position;
    }

    public static PoulePosition Create(Poule poule, short position)
    {
        ArgumentNullException.ThrowIfNull(poule);
        if (position < 1 || position > poule.TotalPlayers.Value)
        {
            throw new ArgumentException($"Value must be between 1 and {poule.TotalPlayers.Value}.", nameof(position));
        }

        return new PoulePosition(poule, position);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Poule;
        yield return Position;
    }
}
