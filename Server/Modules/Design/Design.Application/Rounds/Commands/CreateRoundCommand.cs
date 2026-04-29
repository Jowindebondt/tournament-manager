using AutoMapper;
using Design.Application.DTOs;
using Design.Domain.Entities;
using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using MediatR;

namespace Design.Application.Rounds.Commands;

public record CreateRoundCommand(string Name, Guid TournamentId) : IRequest<RoundDto>;

public class CreateRoundCommandHandler(IMapper mapper, IRoundRepository roundRepository, ITournamentRepository tournamentRepository)
    : IRequestHandler<CreateRoundCommand, RoundDto>
{
    public async Task<RoundDto> Handle(CreateRoundCommand request, CancellationToken cancellationToken)
    {
        var tournament = await tournamentRepository.GetByIdAsync(new TournamentId(request.TournamentId))
            ?? throw new ArgumentException("Tournament not found");

        var roundId = new RoundId(Guid.NewGuid());
        var roundName = RoundName.Create(request.Name);
        var round = new Round(roundId, roundName, tournament.Id);

        await roundRepository.AddAsync(round);

        return mapper.Map<RoundDto>(round);
    }
}
