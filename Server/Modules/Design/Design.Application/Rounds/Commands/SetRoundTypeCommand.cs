using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using MediatR;

namespace Design.Application.Rounds.Commands;

public record SetRoundTypeCommand(Guid Id, RoundType Type) : IRequest;

public class SetRoundTypeCommandHandler(IRoundRepository roundRepository)
    : IRequestHandler<SetRoundTypeCommand>
{
    public async Task Handle(SetRoundTypeCommand request, CancellationToken cancellationToken)
    {
        var round = await roundRepository.GetByIdAsync(new RoundId(request.Id))
            ?? throw new ArgumentException("Round not found");

        round.SetType(request.Type);

        await roundRepository.UpdateAsync(round);
    }
}
