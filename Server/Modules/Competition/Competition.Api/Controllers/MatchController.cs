using AutoMapper;
using Competition.Api.ViewModels;
using Competition.Application.Matches.Commands;
using Competition.Application.Matches.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Competition.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MatchController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public MatchController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet($"/api/Poule/{{{nameof(pouleId)}}}/matches")]
    public async Task<IActionResult> GetAllByPouleAsync([FromRoute] Guid pouleId)
    {
        var matches = await _mediator.Send(new GetMatchesByPouleQuery(pouleId));
        return Ok(_mapper.Map<IEnumerable<MatchViewModel>>(matches));
    }

    [HttpGet($"{{{nameof(id)}}}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var match = await _mediator.Send(new GetMatchByIdQuery(id));
        if (match == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<MatchViewModel>(match));
    }

    [HttpPost($"{{{nameof(id)}}}/saveresult")]
    public async Task<IActionResult> SaveResultAsync([FromRoute] Guid id, [FromBody] SaveMatchResultViewModel saveMatchResult)
    {
        await _mediator.Send(new SaveMatchResultCommand(id, saveMatchResult.Player1Score, saveMatchResult.Player2Score));
        return NoContent();
    }
}

