using System.Diagnostics;
using Design.Domain.ValueObjects;

namespace Sports.TableTennis.Domain.ValueObjects;

[DebuggerDisplay($"{nameof(BestOf)} = {{{nameof(BestOf)}}}")]
public sealed class TableTennisRoundSettings : RoundSettings
{
    public short BestOf { get; }

    private TableTennisRoundSettings(short bestOf)
    {
        BestOf = bestOf;
    }

    public static TableTennisRoundSettings Create(short bestOf)
    {
        if (bestOf <= 0)
        {
            throw new ArgumentException("Value must be bigger than 0.", nameof(bestOf));
        }
        if (int.IsEvenInteger(bestOf))
        {
            throw new ArgumentException("Value must be an odd number.", nameof(bestOf));
        }

        return new TableTennisRoundSettings(bestOf);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return BestOf;
    }
}
