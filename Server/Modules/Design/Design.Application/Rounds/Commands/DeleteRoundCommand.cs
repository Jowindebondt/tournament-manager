using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using MediatR;

namespace Design.Application.Rounds.Commands;

public record DeleteRoundCommand(Guid Id) : IRequest;

public class DeleteRoundCommandHandler(IRoundRepository roundRepository)
    : IRequestHandler<DeleteRoundCommand>
{
    public async Task Handle(DeleteRoundCommand request, CancellationToken cancellationToken)
    {
        var round = await roundRepository.GetByIdAsync(new RoundId(request.Id))
            ?? throw new ArgumentException("Round not found");

        await roundRepository.RemoveAsync(round);
    }
}
