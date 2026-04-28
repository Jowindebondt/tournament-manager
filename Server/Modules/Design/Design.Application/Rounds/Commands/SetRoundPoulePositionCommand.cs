using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using MediatR;

namespace Design.Application.Rounds.Commands;

public record SetRoundPoulePositionCommand(
    Guid Id,
    Guid PreviousPouleId,
    int PreviousPosition,
    Guid CurrentPouleId,
    int CurrentPosition) : IRequest;

public class SetRoundPoulePositionCommandHandler(IRoundRepository roundRepository, IPouleRepository pouleRepository)
    : IRequestHandler<SetRoundPoulePositionCommand>
{
    public async Task Handle(SetRoundPoulePositionCommand request, CancellationToken cancellationToken)
    {
        var round = await roundRepository.GetByIdAsync(new RoundId(request.Id))
            ?? throw new ArgumentException("Round not found");

        if (round.PreviousRound == null)
        {
            throw new ArgumentException("Round has no previous round configured");
        }

        var currentPoule = await pouleRepository.GetByIdAsync(new PouleId(request.CurrentPouleId))
            ?? throw new ArgumentException("Current poule not found");

        if (currentPoule.RoundId.Value != round.Id.Value)
        {
            throw new ArgumentException("Current poule is not part of round");
        }

        var previousPoule = await pouleRepository.GetByIdAsync(new PouleId(request.PreviousPouleId))
            ?? throw new ArgumentException("Previous poule not found");

        if (previousPoule.RoundId.Value != round.PreviousRound.Id.Value)
        {
            throw new ArgumentException("Previous poule is not part of previous round");
        }

        var currentPoulePosition = PoulePosition.Create(currentPoule, (short)request.CurrentPosition);
        var previousPoulePosition = PoulePosition.Create(previousPoule, (short)request.PreviousPosition);
        PoulePositionMapping.Create(previousPoulePosition, currentPoulePosition, round.Settings);

        await roundRepository.UpdateAsync(round);
    }
}
