namespace Competition.Domain.ValueObjects;

public sealed class RoundRobinPlan : CompetitionPlan
{
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield break;
    }
}
