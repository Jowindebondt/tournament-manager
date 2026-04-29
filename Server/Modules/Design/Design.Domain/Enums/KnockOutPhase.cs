using System.ComponentModel;

namespace Design.Domain.Enums;

public enum KnockOutPhase
{
    [Description(nameof(Final))]
    Final,

    [Description(nameof(SemiFinal))]
    SemiFinal,

    [Description(nameof(QuarterFinal))]
    QuarterFinal,

    [Description(nameof(RoundOf16))]
    RoundOf16,

    [Description(nameof(RoundOf32))]
    RoundOf32,
}
