using AutoMapper;
using Design.Api.ViewModels;
using Design.Application.DTOs;
using Design.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Design.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoundController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly RoundService _roundService;

    public RoundController(IMapper mapper, RoundService roundService)
    {
        _mapper = mapper;
        _roundService = roundService;
    }

    [HttpGet($"/api/Tournament/{{{nameof(tournamentId)}}}/rounds")]
    public async Task<IActionResult> GetAllByTournamentAsync([FromRoute]Guid tournamentId)
    {
        var rounds = await _roundService.GetAllByTournamentAsync(tournamentId);
        return Ok(_mapper.Map<IEnumerable<RoundViewModel>>(rounds));
    }

    [HttpGet($"{{{nameof(id)}}}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute]Guid id)
    {
        var round = await _roundService.GetByIdAsync(id);
        if (round == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<RoundViewModel>(round));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody]CreateRoundViewModel createRound)
    {
        var round = await _roundService.CreateAsync(_mapper.Map<CreateRoundDto>(createRound));
        return CreatedAtAction(nameof(GetByIdAsync), new { id = round.Id }, _mapper.Map<RoundViewModel>(round));
    }

    [HttpPost($"{{{nameof(id)}}}/rename")]
    public async Task<IActionResult> RenameAsync([FromRoute]Guid id, [FromBody]RenameRoundViewModel renameRound)
    {
        var renameDto = _mapper.Map<RenameRoundDto>(renameRound, opt => opt.AfterMap((src, dest) => dest.Id = id));
        await _roundService.RenameAsync(renameDto);
        return NoContent();
    }

    [HttpPost($"{{{nameof(id)}}}/setpreviousround")]
    public async Task<IActionResult> SetPreviousRoundAsync([FromRoute]Guid id, [FromBody]SetPreviousRoundViewModel setPreviousRound)
    {
        var setPreviousRoundDto = _mapper.Map<SetPreviousRoundDto>(setPreviousRound, opt => opt.AfterMap((src, dest) => dest.Id = id));
        await _roundService.SetPreviousRoundAsync(setPreviousRoundDto);
        return NoContent();
    }

    [HttpPost($"{{{nameof(id)}}}/settabletennissettings")]
    public async Task<IActionResult> SetTableTennisSettingsAsync([FromRoute]Guid id, [FromBody]SetTableTennisSettingsRoundViewModel setTableTennisSettings)
    {
        var setTableTennisSettingsDto = _mapper.Map<SetTableTennisRoundSettingsDto>(setTableTennisSettings, opt => opt.AfterMap((src, dest) => dest.Id = id));
        await _roundService.SetTableTennisSettingsAsync(setTableTennisSettingsDto);
        return NoContent();
    }

    [HttpPost($"{{{nameof(id)}}}/setroundpoulepositions")]
    public async Task<IActionResult> SetRoundPoulePositions([FromRoute]Guid id, [FromBody]IEnumerable<SetRoundPoulePositionViewModel> setRoundPoulePositions)
    {
        foreach (var setRoundPoulePosition in setRoundPoulePositions)
        {
            var setTableTennisSettingsDto = _mapper.Map<SetRoundPoulePositionDto>(setRoundPoulePosition, opt => opt.AfterMap((src, dest) => dest.Id = id));
            await _roundService.SetRoundPoulePositionAsync(setTableTennisSettingsDto);
        }
        return NoContent();
    }

    [HttpDelete($"{{{nameof(id)}}}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        await _roundService.DeleteAsync(id);
        return NoContent();
    }
}
