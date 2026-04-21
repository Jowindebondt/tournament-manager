using System.Diagnostics;
using CSharpFunctionalExtensions;

namespace Design.Domain.ValueObjects;

[DebuggerDisplay($"{nameof(Previous)} = {{{nameof(Previous)}}}, {nameof(Current)} = {{{nameof(Current)}}}, {nameof(RoundSettings)} = {{{nameof(RoundSettings)}}}")]
public sealed class PoulePositionMapping : ValueObject
{
    public PoulePosition Previous { get; }
    public PoulePosition Current { get; }
    public RoundSettings RoundSettings { get; }

    private PoulePositionMapping(PoulePosition previous, PoulePosition current, RoundSettings roundSettings)
    {
        Previous = previous;
        Current = current;
        RoundSettings = roundSettings;
    }

    public static PoulePositionMapping Create(PoulePosition previous, PoulePosition current, RoundSettings roundSettings)
    {
        ArgumentNullException.ThrowIfNull(previous);
        ArgumentNullException.ThrowIfNull(current);
        ArgumentNullException.ThrowIfNull(roundSettings);

        return new PoulePositionMapping(previous, current, roundSettings);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Previous;
        yield return Current;
        yield return RoundSettings;
    }
}
