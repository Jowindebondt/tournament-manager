using System.Diagnostics;
using CSharpFunctionalExtensions;

namespace Design.Domain.ValueObjects;

[DebuggerDisplay($"{nameof(Value)} = {{{nameof(Value)}}}")]
public sealed class RoundName : ValueObject
{
    public const int MaxLength = 100;

    public string Value { get; }

    private RoundName(string value)
    {
        Value = value;
    }

    public static RoundName Create(string value)
    {
        if (value.Length is 0 or > MaxLength)
        {
            throw new ArgumentException($"Length must be between 1 and {MaxLength}.", nameof(value));
        }

        return new RoundName(value);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}
