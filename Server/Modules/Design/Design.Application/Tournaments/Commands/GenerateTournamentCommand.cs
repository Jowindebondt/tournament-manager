using MediatR;

namespace Design.Application.Tournaments.Commands;

public record GenerateTournamentCommand(Guid Id) : IRequest;

public class GenerateTournamentCommandHandler : IRequestHandler<GenerateTournamentCommand>
{
    public Task Handle(GenerateTournamentCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement tournament generation logic
        return Task.CompletedTask;
    }
}
