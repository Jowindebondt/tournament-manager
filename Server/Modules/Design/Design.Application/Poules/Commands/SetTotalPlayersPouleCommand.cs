using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using MediatR;

namespace Design.Application.Poules.Commands;

public record SetTotalPlayersPouleCommand(Guid Id, int TotalPlayers) : IRequest;

public class SetTotalPlayersPouleCommandHandler(IPouleRepository pouleRepository)
    : IRequestHandler<SetTotalPlayersPouleCommand>
{
    public async Task Handle(SetTotalPlayersPouleCommand request, CancellationToken cancellationToken)
    {
        var poule = await pouleRepository.GetByIdAsync(new PouleId(request.Id))
            ?? throw new ArgumentException("Poule not found");

        poule.SetTotalPlayers(PoulePlayersCount.Create((short)request.TotalPlayers));

        await pouleRepository.UpdateAsync(poule);
    }
}
