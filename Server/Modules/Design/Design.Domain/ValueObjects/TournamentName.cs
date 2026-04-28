using System.Diagnostics;
using CSharpFunctionalExtensions;

namespace Design.Domain.ValueObjects;

[DebuggerDisplay($"{nameof(Value)} = {{{nameof(Value)}}}")]
public sealed class TournamentName : ValueObject
{
    public const int MaxLength = 100;

    public string Value { get; }

    private TournamentName(string value)
    {
        Value = value;
    }

    public static TournamentName Create(string value)
    {
        if (value.Length is 0 or > MaxLength)
        {
            throw new ArgumentException($"Length must be between 1 and {MaxLength}.", nameof(value));
        }

        return new TournamentName(value);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}
