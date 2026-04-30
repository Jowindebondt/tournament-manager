using AutoMapper;
using Competition.Application.DTOs;
using Competition.Domain.Interfaces;
using Competition.Domain.ValueObjects;
using MediatR;

namespace Competition.Application.Matches.Queries;

public record GetMatchByIdQuery(Guid Id) : IRequest<MatchDto?>;

public class GetMatchByIdQueryHandler(IMapper mapper, IMatchRepository matchRepository)
    : IRequestHandler<GetMatchByIdQuery, MatchDto?>
{
    public async Task<MatchDto?> Handle(GetMatchByIdQuery request, CancellationToken cancellationToken)
    {
        var match = await matchRepository.GetByIdAsync(new MatchId(request.Id));
        return mapper.Map<MatchDto?>(match);
    }
}
