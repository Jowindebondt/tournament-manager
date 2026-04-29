namespace Design.Domain.ValueObjects;

public sealed class RoundRobinType : RoundType
{
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield break;
    }
}
