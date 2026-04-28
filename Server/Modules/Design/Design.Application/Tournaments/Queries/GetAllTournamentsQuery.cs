using AutoMapper;
using Design.Application.DTOs;
using Design.Domain.Interfaces;
using MediatR;

namespace Design.Application.Tournaments.Queries;

public record GetAllTournamentsQuery : IRequest<IEnumerable<TournamentDto>>;

public class GetAllTournamentsQueryHandler(IMapper mapper, ITournamentRepository tournamentRepository)
    : IRequestHandler<GetAllTournamentsQuery, IEnumerable<TournamentDto>>
{
    public async Task<IEnumerable<TournamentDto>> Handle(GetAllTournamentsQuery request, CancellationToken cancellationToken)
    {
        var tournaments = await tournamentRepository.GetAllAsync();
        return mapper.Map<IEnumerable<TournamentDto>>(tournaments);
    }
}
