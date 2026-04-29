using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using MediatR;

namespace Design.Application.Poules.Commands;

public record DeletePouleCommand(Guid Id) : IRequest;

public class DeletePouleCommandHandler(IPouleRepository pouleRepository)
    : IRequestHandler<DeletePouleCommand>
{
    public async Task Handle(DeletePouleCommand request, CancellationToken cancellationToken)
    {
        var poule = await pouleRepository.GetByIdAsync(new PouleId(request.Id))
            ?? throw new ArgumentException("Poule not found");

        await pouleRepository.RemoveAsync(poule);
    }
}
