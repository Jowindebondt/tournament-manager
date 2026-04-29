using Competition.Domain.Enums;

namespace Competition.Domain.ValueObjects;

public sealed class KnockOutPlan : RoundPlan
{
    public KnockOutPhase Phase { get; }

    public KnockOutPlan(KnockOutPhase phase)
    {
        Phase = phase;
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Phase;
    }
}
