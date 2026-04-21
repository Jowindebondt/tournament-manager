using AutoMapper;
using Design.Api.ViewModels;
using Design.Application.DTOs;
using Design.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Design.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PouleController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPouleService _pouleService;

    public PouleController(IMapper mapper, IPouleService pouleService)
    {
        _mapper = mapper;
        _pouleService = pouleService;
    }

    [HttpGet($"/api/Tournament/{{{nameof(tournamentId)}}}/Round/{{{nameof(roundId)}}}/poules")]
    public async Task<IActionResult> GetAllByTournamentAndRoundAsync([FromRoute]Guid tournamentId, [FromRoute]Guid roundId)
    {
        var poules = await _pouleService.GetAllByRoundAndTournamentAsync(roundId, tournamentId);
        return Ok(_mapper.Map<IEnumerable<PouleViewModel>>(poules));
    }

    [HttpGet($"{{{nameof(id)}}}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute]Guid id)
    {
        var poule = await _pouleService.GetByIdAsync(id);
        if (poule == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<PouleViewModel>(poule));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody]CreatePouleViewModel createPoule)
    {
        var poule = await _pouleService.CreateAsync(_mapper.Map<CreatePouleDto>(createPoule));
        return CreatedAtAction(nameof(GetByIdAsync), new { id = poule.Id }, _mapper.Map<PouleViewModel>(poule));
    }

    [HttpPost($"{{{nameof(id)}}}/rename")]
    public async Task<IActionResult> RenameAsync([FromRoute]Guid id, [FromBody]RenamePouleViewModel renamePoule)
    {
        var renameDto = _mapper.Map<RenamePouleDto>(renamePoule, opt => opt.AfterMap((src, dest) => dest.Id = id));
        await _pouleService.RenameAsync(renameDto);
        return NoContent();
    }

    [HttpPost($"{{{nameof(id)}}}/settotalplayers")]
    public async Task<IActionResult> SetTotalPlayersAsync([FromRoute]Guid id, [FromBody]SetTotalPlayersPouleViewModel setTotalPlayersPoule)
    {
        var setTotalPlayersDto = _mapper.Map<SetTotalPlayersPouleDto>(setTotalPlayersPoule, opt => opt.AfterMap((src, dest) => dest.Id = id));
        await _pouleService.SetTotalPlayersAsync(setTotalPlayersDto);
        return NoContent();
    }

    [HttpDelete($"{{{nameof(id)}}}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        await _pouleService.DeleteAsync(id);
        return NoContent();
    }
}
