using AutoMapper;
using Design.Api.ViewModels;
using Design.Application.Tournaments.Commands;
using Design.Application.Tournaments.Queries;
using Design.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Design.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TournamentController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public TournamentController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var tournaments = await _mediator.Send(new GetAllTournamentsQuery());
        return Ok(_mapper.Map<IEnumerable<TournamentViewModel>>(tournaments));
    }

    [HttpGet($"{{{nameof(id)}}}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var tournament = await _mediator.Send(new GetTournamentByIdQuery(id));
        if (tournament == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<TournamentViewModel>(tournament));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateTournamentViewModel createTournament)
    {
        if (!Enum.TryParse<Sport>(createTournament.Sport, ignoreCase: true, out var sport))
        {
            return BadRequest($"Invalid sport value: '{createTournament.Sport}'.");
        }
        var tournament = await _mediator.Send(new CreateTournamentCommand(createTournament.Name, sport));
        return CreatedAtAction(nameof(GetByIdAsync), new { id = tournament.Id }, _mapper.Map<TournamentViewModel>(tournament));
    }

    [HttpPost($"{{{nameof(id)}}}/rename")]
    public async Task<IActionResult> RenameAsync([FromRoute] Guid id, [FromBody] RenameTournamentViewModel renameTournament)
    {
        await _mediator.Send(new RenameTournamentCommand(id, renameTournament.Name));
        return NoContent();
    }

    [HttpDelete($"{{{nameof(id)}}}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        await _mediator.Send(new DeleteTournamentCommand(id));
        return NoContent();
    }
}
