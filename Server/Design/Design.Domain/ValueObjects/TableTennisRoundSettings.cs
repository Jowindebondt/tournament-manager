
using System.Diagnostics;

namespace Design.Domain.ValueObjects;

[DebuggerDisplay($"{nameof(BestOf)} = {{{nameof(BestOf)}}}")]
public sealed class TableTennisRoundSettings : RoundSettings
{
    public int BestOf { get; }

    private TableTennisRoundSettings(int bestOf)
    {
        BestOf = bestOf;
    }

    public static TableTennisRoundSettings Create(int bestOf)
    {
        if (int.IsEvenInteger(bestOf))
        {
            throw new ArgumentException("Value must be an odd number", nameof(bestOf));
        }
        if (bestOf <= 0)
        {
            throw new ArgumentException("Value must be bigger than 0", nameof(bestOf));
        }
        return new TableTennisRoundSettings(bestOf);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return BestOf;
    }
}
