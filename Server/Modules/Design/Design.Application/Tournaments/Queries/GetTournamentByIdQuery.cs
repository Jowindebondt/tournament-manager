using AutoMapper;
using Design.Application.DTOs;
using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using MediatR;

namespace Design.Application.Tournaments.Queries;

public record GetTournamentByIdQuery(Guid Id) : IRequest<TournamentDto?>;

public class GetTournamentByIdQueryHandler(IMapper mapper, ITournamentRepository tournamentRepository)
    : IRequestHandler<GetTournamentByIdQuery, TournamentDto?>
{
    public async Task<TournamentDto?> Handle(GetTournamentByIdQuery request, CancellationToken cancellationToken)
    {
        var tournament = await tournamentRepository.GetByIdAsync(new TournamentId(request.Id));
        return mapper.Map<TournamentDto?>(tournament);
    }
}
