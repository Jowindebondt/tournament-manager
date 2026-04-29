using Competition.Domain.Interfaces;
using Competition.Domain.ValueObjects;
using MediatR;

namespace Competition.Application.Matches.Commands;

public record SaveMatchResultCommand(Guid MatchId, short Player1Score, short Player2Score) : IRequest;

public class SaveMatchResultCommandHandler(IMatchRepository matchRepository)
    : IRequestHandler<SaveMatchResultCommand>
{
    public async Task Handle(SaveMatchResultCommand request, CancellationToken cancellationToken)
    {
        var match = await matchRepository.GetByIdAsync(new MatchId(request.MatchId))
            ?? throw new ArgumentException("Match not found");

        var result = new MatchResult(request.Player1Score, request.Player2Score);
        match.SaveResult(result);

        await matchRepository.UpdateAsync(match);
    }
}
