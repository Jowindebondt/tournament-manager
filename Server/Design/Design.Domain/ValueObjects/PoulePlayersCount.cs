using System.Diagnostics;
using CSharpFunctionalExtensions;

namespace Design.Domain.ValueObjects;

[DebuggerDisplay($"{nameof(Value)} = {{{nameof(Value)}}}")]
public sealed class PoulePlayersCount : ValueObject
{
    public const int MaxValue = 12;

    public int Value { get; }

    private PoulePlayersCount(int value)
    {
        Value = value;
    }

    public static PoulePlayersCount Create(int value)
    {
        if (value is < 3 or > MaxValue)
        {
            throw new ArgumentException($"Value must be between 3 and {MaxValue}.", nameof(value));
        }
        return new PoulePlayersCount(value);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}
