using CSharpFunctionalExtensions;

namespace Competition.Domain.ValueObjects;

public sealed class MatchResult : ValueObject
{
    public short Player1Score { get; }
    public short Player2Score { get; }

    public MatchResult(short player1Score, short player2Score)
    {
        if (player1Score < 0)
        {
            throw new ArgumentException("Value must be zero or greater.", nameof(player1Score));
        }
        if (player2Score < 0)
        {
            throw new ArgumentException("Value must be zero or greater.", nameof(player2Score));
        }

        Player1Score = player1Score;
        Player2Score = player2Score;
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Player1Score;
        yield return Player2Score;
    }
}
