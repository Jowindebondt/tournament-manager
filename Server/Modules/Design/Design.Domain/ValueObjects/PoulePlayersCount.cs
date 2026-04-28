using System.Diagnostics;
using CSharpFunctionalExtensions;

namespace Design.Domain.ValueObjects;

[DebuggerDisplay($"{nameof(Value)} = {{{nameof(Value)}}}")]
public sealed class PoulePlayersCount : ValueObject
{
    public const short MaxValue = 12;
    public const short MinValue = 3;

    public short Value { get; }

    private PoulePlayersCount(short value)
    {
        Value = value;
    }

    public static PoulePlayersCount Create(short value)
    {
        if (value < MinValue || value > MaxValue)
        {
            throw new ArgumentException($"Value must be between {MinValue} and {MaxValue}.", nameof(value));
        }

        return new PoulePlayersCount(value);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}
