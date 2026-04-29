using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using MediatR;

namespace Design.Application.Rounds.Commands;

public record SetPreviousRoundCommand(Guid Id, Guid PreviousId) : IRequest;

public class SetPreviousRoundCommandHandler(IRoundRepository roundRepository)
    : IRequestHandler<SetPreviousRoundCommand>
{
    public async Task Handle(SetPreviousRoundCommand request, CancellationToken cancellationToken)
    {
        var round = await roundRepository.GetByIdAsync(new RoundId(request.Id))
            ?? throw new ArgumentException("Round not found");

        var previousRound = await roundRepository.GetByIdAsync(new RoundId(request.PreviousId))
            ?? throw new ArgumentException("Previous round not found");

        round.SetPreviousRound(previousRound);

        await roundRepository.UpdateAsync(round);
    }
}
