using AutoMapper;
using Design.Application.DTOs;
using Design.Domain.Entities;
using Design.Domain.Enums;
using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using MediatR;

namespace Design.Application.Tournaments.Commands;

public record CreateTournamentCommand(string Name, Sport Sport) : IRequest<TournamentDto>;

public class CreateTournamentCommandHandler(IMapper mapper, ITournamentRepository tournamentRepository)
    : IRequestHandler<CreateTournamentCommand, TournamentDto>
{
    public async Task<TournamentDto> Handle(CreateTournamentCommand request, CancellationToken cancellationToken)
    {
        var tournamentId = new TournamentId(Guid.NewGuid());
        var tournamentName = TournamentName.Create(request.Name);
        var tournament = new Tournament(tournamentId, tournamentName, request.Sport);

        await tournamentRepository.AddAsync(tournament);

        return mapper.Map<TournamentDto>(tournament);
    }
}
