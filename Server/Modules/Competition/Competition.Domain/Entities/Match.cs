using CSharpFunctionalExtensions;
using Competition.Domain.ValueObjects;

namespace Competition.Domain.Entities;

public sealed class Match : Entity<MatchId>
{
    public short Player1Index { get; private set; }
    public short Player2Index { get; private set; }
    public MatchResult? Result { get; private set; }

    public PouleId PouleId { get; private set; }
    public Poule Poule { get; private set; } = null!;

    public Match(MatchId id, short player1Index, short player2Index, PouleId pouleId) : base(id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        ArgumentNullException.ThrowIfNull(pouleId, nameof(pouleId));

        if (player1Index <= 0)
        {
            throw new ArgumentException("Value must be greater than zero.", nameof(player1Index));
        }
        if (player2Index <= 0)
        {
            throw new ArgumentException("Value must be greater than zero.", nameof(player2Index));
        }
        if (player1Index == player2Index)
        {
            throw new ArgumentException("Player indices must differ.", nameof(player2Index));
        }

        Player1Index = player1Index;
        Player2Index = player2Index;
        PouleId = pouleId;
    }

    public void SaveResult(MatchResult result)
    {
        ArgumentNullException.ThrowIfNull(result, nameof(result));

        Result = result;
    }
}
