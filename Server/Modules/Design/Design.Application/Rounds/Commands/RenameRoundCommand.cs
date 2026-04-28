using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using MediatR;

namespace Design.Application.Rounds.Commands;

public record RenameRoundCommand(Guid Id, string Name) : IRequest;

public class RenameRoundCommandHandler(IRoundRepository roundRepository)
    : IRequestHandler<RenameRoundCommand>
{
    public async Task Handle(RenameRoundCommand request, CancellationToken cancellationToken)
    {
        var round = await roundRepository.GetByIdAsync(new RoundId(request.Id))
            ?? throw new ArgumentException("Round not found");

        round.Rename(RoundName.Create(request.Name));

        await roundRepository.UpdateAsync(round);
    }
}
