using System.Diagnostics;
using Design.Domain.Enums;

namespace Design.Domain.ValueObjects;

[DebuggerDisplay($"{nameof(Phase)} = {{{nameof(Phase)}}}")]
public sealed class KnockOutType : RoundType
{
    public KnockOutPhase Phase { get; }

    public KnockOutType(KnockOutPhase phase)
    {
        Phase = phase;
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Phase;
    }
}
