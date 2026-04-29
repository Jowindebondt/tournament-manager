using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using MediatR;

namespace Design.Application.Poules.Commands;

public record RenamePouleCommand(Guid Id, string Name) : IRequest;

public class RenamePouleCommandHandler(IPouleRepository pouleRepository)
    : IRequestHandler<RenamePouleCommand>
{
    public async Task Handle(RenamePouleCommand request, CancellationToken cancellationToken)
    {
        var poule = await pouleRepository.GetByIdAsync(new PouleId(request.Id))
            ?? throw new ArgumentException("Poule not found");

        poule.Rename(PouleName.Create(request.Name));

        await pouleRepository.UpdateAsync(poule);
    }
}
