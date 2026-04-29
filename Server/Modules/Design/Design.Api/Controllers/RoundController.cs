using AutoMapper;
using Design.Api.ViewModels;
using Design.Application.Rounds.Commands;
using Design.Application.Rounds.Queries;
using Design.Domain.Enums;
using Design.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sports.TableTennis.Domain.ValueObjects;

namespace Design.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoundController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public RoundController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet($"/api/Tournament/{{{nameof(tournamentId)}}}/rounds")]
    public async Task<IActionResult> GetAllByTournamentAsync([FromRoute] Guid tournamentId)
    {
        var rounds = await _mediator.Send(new GetAllRoundsByTournamentQuery(tournamentId));
        return Ok(_mapper.Map<IEnumerable<RoundViewModel>>(rounds));
    }

    [HttpGet($"{{{nameof(id)}}}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var round = await _mediator.Send(new GetRoundByIdQuery(id));
        if (round == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<RoundViewModel>(round));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateRoundViewModel createRound)
    {
        var round = await _mediator.Send(new CreateRoundCommand(createRound.Name, createRound.TournamentId));
        return CreatedAtAction(nameof(GetByIdAsync), new { id = round.Id }, _mapper.Map<RoundViewModel>(round));
    }

    [HttpPost($"{{{nameof(id)}}}/rename")]
    public async Task<IActionResult> RenameAsync([FromRoute] Guid id, [FromBody] RenameRoundViewModel renameRound)
    {
        await _mediator.Send(new RenameRoundCommand(id, renameRound.Name));
        return NoContent();
    }

    [HttpPost($"{{{nameof(id)}}}/setpreviousround")]
    public async Task<IActionResult> SetPreviousRoundAsync([FromRoute] Guid id, [FromBody] SetPreviousRoundViewModel setPreviousRound)
    {
        await _mediator.Send(new SetPreviousRoundCommand(id, setPreviousRound.PreviousId));
        return NoContent();
    }

    [HttpPost($"{{{nameof(id)}}}/settabletennissettings")]
    public async Task<IActionResult> SetTableTennisSettingsAsync([FromRoute] Guid id, [FromBody] SetTableTennisSettingsRoundViewModel setTableTennisSettings)
    {
        var settings = TableTennisRoundSettings.Create((short)setTableTennisSettings.BestOf);
        await _mediator.Send(new SetRoundSettingsCommand(id, settings));
        return NoContent();
    }

    [HttpPost($"{{{nameof(id)}}}/setroundtype")]
    public async Task<IActionResult> SetRoundTypeAsync([FromRoute] Guid id, [FromBody] SetRoundTypeViewModel setRoundType)
    {
        RoundType roundType = setRoundType.Type switch
        {
            "RoundRobin" => RoundRobinType.Instance,
            "KnockOut" when setRoundType.KnockOutPhase != null && Enum.TryParse<KnockOutPhase>(setRoundType.KnockOutPhase, out var phase) => new KnockOutType(phase),
            "KnockOut" => throw new ArgumentException($"A valid KnockOutPhase is required for KnockOut round type. Valid values: {string.Join(", ", Enum.GetNames<KnockOutPhase>())}."),
            _ => throw new ArgumentException($"Unsupported round type '{setRoundType.Type}'.")
        };
        await _mediator.Send(new SetRoundTypeCommand(id, roundType));
        return NoContent();
    }

    [HttpPost($"{{{nameof(id)}}}/setroundpoulepositions")]
    public async Task<IActionResult> SetRoundPoulePositions([FromRoute] Guid id, [FromBody] IEnumerable<SetRoundPoulePositionViewModel> setRoundPoulePositions)
    {
        foreach (var setRoundPoulePosition in setRoundPoulePositions)
        {
            await _mediator.Send(new SetRoundPoulePositionCommand(
                id,
                setRoundPoulePosition.PreviousPouleId,
                setRoundPoulePosition.PreviousPosition,
                setRoundPoulePosition.CurrentPouleId,
                setRoundPoulePosition.CurrentPosition));
        }
        return NoContent();
    }

    [HttpDelete($"{{{nameof(id)}}}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        await _mediator.Send(new DeleteRoundCommand(id));
        return NoContent();
    }
}
