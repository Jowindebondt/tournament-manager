using System.Diagnostics;
using CSharpFunctionalExtensions;

namespace Competition.Domain.ValueObjects;

[DebuggerDisplay($"{nameof(Value)} = {{{nameof(Value)}}}")]
public sealed class RoundId : ValueObject
{
    public Guid Value { get; }

    public RoundId(Guid value)
    {
        Value = value;
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}
