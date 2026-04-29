using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using MediatR;

namespace Design.Application.Rounds.Commands;

public record SetRoundSettingsCommand(Guid Id, RoundSettings Settings) : IRequest;

public class SetRoundSettingsCommandHandler(IRoundRepository roundRepository)
    : IRequestHandler<SetRoundSettingsCommand>
{
    public async Task Handle(SetRoundSettingsCommand request, CancellationToken cancellationToken)
    {
        var round = await roundRepository.GetByIdAsync(new RoundId(request.Id))
            ?? throw new ArgumentException("Round not found");

        round.SetSettings(request.Settings);

        await roundRepository.UpdateAsync(round);
    }
}
