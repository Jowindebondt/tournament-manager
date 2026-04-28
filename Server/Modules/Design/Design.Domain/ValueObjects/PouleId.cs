using System.Diagnostics;
using CSharpFunctionalExtensions;

namespace Design.Domain.ValueObjects;

[DebuggerDisplay($"{nameof(Value)} = {{{nameof(Value)}}}")]
public sealed class PouleId : ValueObject
{
    public Guid Value { get; }

    public PouleId(Guid value)
    {
        Value = value;
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}
