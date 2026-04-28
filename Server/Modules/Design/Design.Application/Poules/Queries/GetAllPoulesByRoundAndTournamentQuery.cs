using AutoMapper;
using Design.Application.DTOs;
using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using MediatR;

namespace Design.Application.Poules.Queries;

public record GetAllPoulesByRoundAndTournamentQuery(Guid RoundId, Guid TournamentId) : IRequest<IEnumerable<PouleDto>>;

public class GetAllPoulesByRoundAndTournamentQueryHandler(IMapper mapper, IPouleRepository pouleRepository)
    : IRequestHandler<GetAllPoulesByRoundAndTournamentQuery, IEnumerable<PouleDto>>
{
    public async Task<IEnumerable<PouleDto>> Handle(GetAllPoulesByRoundAndTournamentQuery request, CancellationToken cancellationToken)
    {
        var poules = await pouleRepository.GetAllByRoundAndTournamentAsync(new TournamentId(request.TournamentId), new RoundId(request.RoundId));
        return mapper.Map<IEnumerable<PouleDto>>(poules);
    }
}
