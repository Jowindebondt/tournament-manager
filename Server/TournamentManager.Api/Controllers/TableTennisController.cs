using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TournamentManager.Application;
using TournamentManager.Domain;

namespace TournamentManager.Api;

[Route("api/[controller]")]
[ApiController]
public class TableTennisController : ControllerBase
{
    private readonly ITournamentService _tournamentService;
    private readonly IRoundService _roundService;

    public TableTennisController(ITournamentService tournamentService, IRoundService roundService)
    {
        _tournamentService = tournamentService;
        _roundService = roundService;
    }

    /// <summary>
    /// Get the settings for the tournament
    /// </summary>
    [HttpGet($"{nameof(GetTournamentSettings)}/{{id}}")]
    public IActionResult GetTournamentSettings([FromRoute]int id)
    {
        return Ok(_tournamentService.GetSettings(id));
        // return Ok(_tournamentService.GetSettings(id) as TableTennisSettings);
    }

    /// <summary>
    /// Set the settings for the tournament
    /// </summary>
    /// <param name="settings">The settings object</param>
    [HttpPost($"{nameof(SetTournamentSettings)}")]
    public IActionResult SetTournamentSettings([FromBody]TableTennisSettings settings)
    {
        if (settings == null)
        {
            return BadRequest("Something went wrong");
        }
        _tournamentService.SetSettings(settings);
        return Ok();
    }

    /// <summary>
    /// Set the settings for the round
    /// </summary>
    /// <param name="settings">The settings object</param>
    [HttpPost($"{nameof(SetRoundSettings)}")]
    public IActionResult SetRoundSettings([FromBody]TableTennisRoundSettings settings)
    {
        if (settings == null)
        {
            return BadRequest("Something went wrong");
        }
        _roundService.SetSettings(settings);
        return Ok();
    }
}
