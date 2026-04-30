using Generation.Application.Tournaments.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Generation.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenerationController : ControllerBase
{
    private readonly IMediator _mediator;

    public GenerationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost($"tournament/{{{nameof(tournamentId)}}}")]
    public async Task<IActionResult> GenerateAsync([FromRoute] Guid tournamentId)
    {
        await _mediator.Send(new GenerateCompetitionCommand(tournamentId));
        return NoContent();
    }
}
