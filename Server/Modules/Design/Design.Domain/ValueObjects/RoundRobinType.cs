namespace Design.Domain.ValueObjects;

public sealed class RoundRobinType : RoundType
{
    public static readonly RoundRobinType Instance = new();

    private RoundRobinType()
    {
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return nameof(RoundRobinType);
    }
}
