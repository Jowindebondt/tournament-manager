using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using MediatR;

namespace Design.Application.Tournaments.Commands;

public record RenameTournamentCommand(Guid Id, string Name) : IRequest;

public class RenameTournamentCommandHandler(ITournamentRepository tournamentRepository)
    : IRequestHandler<RenameTournamentCommand>
{
    public async Task Handle(RenameTournamentCommand request, CancellationToken cancellationToken)
    {
        var tournament = await tournamentRepository.GetByIdAsync(new TournamentId(request.Id))
            ?? throw new ArgumentException("Tournament not found");

        tournament.Rename(TournamentName.Create(request.Name));

        await tournamentRepository.UpdateAsync(tournament);
    }
}
