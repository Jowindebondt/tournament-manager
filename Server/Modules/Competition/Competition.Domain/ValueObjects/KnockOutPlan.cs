using System.Diagnostics;
using Competition.Domain.Enums;

namespace Competition.Domain.ValueObjects;

[DebuggerDisplay($"{nameof(Phase)} = {{{nameof(Phase)}}}")]
public sealed class KnockOutPlan : CompetitionPlan
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
