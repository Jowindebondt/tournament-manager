using Design.Application.Rounds.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sports.TableTennis.Api.ViewModels;
using Sports.TableTennis.Domain.ValueObjects;

namespace Sports.TableTennis.Api.Controllers;

/// <summary>
/// Table Tennis sport-specific round settings.
/// Kept in Sports.TableTennis.Api so that the Design module stays agnostic of
/// sport-specific implementations, preserving the correct dependency direction:
/// Sports.TableTennis → Design, never Design → Sports.TableTennis.
/// </summary>
[Route("api/Round")]
[ApiController]
public class TableTennisRoundController : ControllerBase
{
    private readonly IMediator _mediator;

    public TableTennisRoundController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost($"{{{nameof(id)}}}/settabletennissettings")]
    public async Task<IActionResult> SetTableTennisSettingsAsync(
        [FromRoute] Guid id,
        [FromBody] SetTableTennisSettingsRoundViewModel setTableTennisSettings)
    {
        var settings = TableTennisRoundSettings.Create((short)setTableTennisSettings.BestOf);
        await _mediator.Send(new SetRoundSettingsCommand(id, settings));
        return NoContent();
    }
}
