using AutoMapper;
using Design.Api.ViewModels;
using Design.Application.DTOs;
using Design.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Design.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TournamentController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITournamentService _tournamentService;

    public TournamentController(IMapper mapper, ITournamentService tournamentService)
    {
        _mapper = mapper;
        _tournamentService = tournamentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var tournaments = await _tournamentService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<TournamentViewModel>>(tournaments));
    }

    [HttpGet($"{{{nameof(id)}}}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute]Guid id)
    {
        var tournament = await _tournamentService.GetByIdAsync(id);
        if (tournament == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<TournamentViewModel>(tournament));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody]CreateTournamentViewModel createTournament)
    {
        var tournament = await _tournamentService.CreateAsync(_mapper.Map<CreateTournamentDTO>(createTournament));
        return CreatedAtAction(nameof(GetByIdAsync), new { id = tournament.Id }, _mapper.Map<TournamentViewModel>(tournament));
    }

    [HttpPost($"{{{nameof(id)}}}/rename")]
    public async Task<IActionResult> RenameAsync([FromRoute]Guid id, [FromBody]RenameTournamentViewModel renameTournament)
    {
        var renameDto = _mapper.Map<RenameTournamentDTO>(renameTournament, opt => opt.AfterMap((src, dest) => dest.Id = id));
        await _tournamentService.RenameAsync(renameDto);
        return NoContent();
    }

    [HttpPost($"{{{nameof(id)}}}/loadtemplate/{{{nameof(templateId)}}}")]
    public async Task<IActionResult> LoadTemplateAsync([FromRoute]Guid id, [FromRoute]Guid templateId)
    {
        await _tournamentService.LoadTemplateAsync(id, templateId);
        return NoContent();
    }

    [HttpPost($"{{{nameof(id)}}}/generate")]
    public async Task<IActionResult> GenerateAsync([FromRoute]Guid id)
    {
        await _tournamentService.GenerateAsync(id);
        return NoContent();
    }

    [HttpDelete($"{{{nameof(id)}}}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        await _tournamentService.DeleteAsync(id);
        return NoContent();
    }
}
