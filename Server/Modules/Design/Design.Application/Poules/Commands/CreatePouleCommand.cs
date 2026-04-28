using AutoMapper;
using Design.Application.DTOs;
using Design.Domain.Entities;
using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using MediatR;

namespace Design.Application.Poules.Commands;

public record CreatePouleCommand(string Name, int TotalPlayers, Guid RoundId) : IRequest<PouleDto>;

public class CreatePouleCommandHandler(IMapper mapper, IPouleRepository pouleRepository, IRoundRepository roundRepository)
    : IRequestHandler<CreatePouleCommand, PouleDto>
{
    public async Task<PouleDto> Handle(CreatePouleCommand request, CancellationToken cancellationToken)
    {
        var round = await roundRepository.GetByIdAsync(new RoundId(request.RoundId))
            ?? throw new ArgumentException("Round not found");

        var pouleId = new PouleId(Guid.NewGuid());
        var pouleName = PouleName.Create(request.Name);
        var pouleTotalPlayers = PoulePlayersCount.Create((short)request.TotalPlayers);
        var poule = new Poule(pouleId, pouleName, pouleTotalPlayers, round.Id);

        await pouleRepository.AddAsync(poule);

        return mapper.Map<PouleDto>(poule);
    }
}
