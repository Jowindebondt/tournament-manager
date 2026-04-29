using AutoMapper;
using Competition.Application.DTOs;
using Competition.Domain.Interfaces;
using Competition.Domain.ValueObjects;
using MediatR;

namespace Competition.Application.Matches.Queries;

public record GetMatchesByPouleQuery(Guid PouleId) : IRequest<ICollection<MatchDto>>;

public class GetMatchesByPouleQueryHandler(IMapper mapper, IMatchRepository matchRepository)
    : IRequestHandler<GetMatchesByPouleQuery, ICollection<MatchDto>>
{
    public async Task<ICollection<MatchDto>> Handle(GetMatchesByPouleQuery request, CancellationToken cancellationToken)
    {
        var matches = await matchRepository.GetAllByPouleAsync(new PouleId(request.PouleId));
        return mapper.Map<ICollection<MatchDto>>(matches);
    }
}
