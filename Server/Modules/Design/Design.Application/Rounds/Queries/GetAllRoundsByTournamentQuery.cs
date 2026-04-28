using AutoMapper;
using Design.Application.DTOs;
using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using MediatR;

namespace Design.Application.Rounds.Queries;

public record GetAllRoundsByTournamentQuery(Guid TournamentId) : IRequest<IEnumerable<RoundDto>>;

public class GetAllRoundsByTournamentQueryHandler(IMapper mapper, IRoundRepository roundRepository)
    : IRequestHandler<GetAllRoundsByTournamentQuery, IEnumerable<RoundDto>>
{
    public async Task<IEnumerable<RoundDto>> Handle(GetAllRoundsByTournamentQuery request, CancellationToken cancellationToken)
    {
        var rounds = await roundRepository.GetAllByTournamentAsync(new TournamentId(request.TournamentId));
        return mapper.Map<IEnumerable<RoundDto>>(rounds);
    }
}
