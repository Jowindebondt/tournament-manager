using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using MediatR;

namespace Design.Application.Tournaments.Commands;

public record DeleteTournamentCommand(Guid Id) : IRequest;

public class DeleteTournamentCommandHandler(ITournamentRepository tournamentRepository)
    : IRequestHandler<DeleteTournamentCommand>
{
    public async Task Handle(DeleteTournamentCommand request, CancellationToken cancellationToken)
    {
        var tournament = await tournamentRepository.GetByIdAsync(new TournamentId(request.Id))
            ?? throw new ArgumentException("Tournament not found");

        await tournamentRepository.RemoveAsync(tournament);
    }
}
