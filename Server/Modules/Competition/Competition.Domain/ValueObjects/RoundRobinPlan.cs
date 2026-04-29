namespace Competition.Domain.ValueObjects;

public sealed class RoundRobinPlan : RoundPlan
{
    public static readonly RoundRobinPlan Instance = new();

    private RoundRobinPlan()
    {
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return nameof(RoundRobinPlan);
    }
}
