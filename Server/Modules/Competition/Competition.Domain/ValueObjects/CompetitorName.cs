using System.Diagnostics;
using CSharpFunctionalExtensions;

namespace Competition.Domain.ValueObjects;

[DebuggerDisplay($"{nameof(Value)} = {{{nameof(Value)}}}")]
public sealed class CompetitorName : ValueObject
{
    public const int MaxLength = 100;

    public string Value { get; }

    private CompetitorName(string value)
    {
        Value = value;
    }

    public static CompetitorName Create(string value)
    {
        if (value.Length is 0 or > MaxLength)
        {
            throw new ArgumentException($"Length must be between 1 and {MaxLength}.", nameof(value));
        }

        return new CompetitorName(value);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}
