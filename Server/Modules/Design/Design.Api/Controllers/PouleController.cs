using AutoMapper;
using Design.Api.ViewModels;
using Design.Application.Poules.Commands;
using Design.Application.Poules.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Design.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PouleController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public PouleController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet($"/api/Tournament/{{{nameof(tournamentId)}}}/Round/{{{nameof(roundId)}}}/poules")]
    public async Task<IActionResult> GetAllByTournamentAndRoundAsync([FromRoute] Guid tournamentId, [FromRoute] Guid roundId)
    {
        var poules = await _mediator.Send(new GetAllPoulesByRoundAndTournamentQuery(roundId, tournamentId));
        return Ok(_mapper.Map<IEnumerable<PouleViewModel>>(poules));
    }

    [HttpGet($"{{{nameof(id)}}}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var poule = await _mediator.Send(new GetPouleByIdQuery(id));
        if (poule == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<PouleViewModel>(poule));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePouleViewModel createPoule)
    {
        var poule = await _mediator.Send(new CreatePouleCommand(createPoule.Name, createPoule.TotalPlayers, createPoule.RoundId));
        return CreatedAtAction(nameof(GetByIdAsync), new { id = poule.Id }, _mapper.Map<PouleViewModel>(poule));
    }

    [HttpPost($"{{{nameof(id)}}}/rename")]
    public async Task<IActionResult> RenameAsync([FromRoute] Guid id, [FromBody] RenamePouleViewModel renamePoule)
    {
        await _mediator.Send(new RenamePouleCommand(id, renamePoule.Name));
        return NoContent();
    }

    [HttpPost($"{{{nameof(id)}}}/settotalplayers")]
    public async Task<IActionResult> SetTotalPlayersAsync([FromRoute] Guid id, [FromBody] SetTotalPlayersPouleViewModel setTotalPlayersPoule)
    {
        await _mediator.Send(new SetTotalPlayersPouleCommand(id, setTotalPlayersPoule.TotalPlayers));
        return NoContent();
    }

    [HttpDelete($"{{{nameof(id)}}}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        await _mediator.Send(new DeletePouleCommand(id));
        return NoContent();
    }
}
