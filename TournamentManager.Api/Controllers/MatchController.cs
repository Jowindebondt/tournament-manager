using Microsoft.AspNetCore.Mvc;
using TournamentManager.Application;
using TournamentManager.Domain;

namespace TournamentManager.Api;

/// <summary>
/// This controller is responsible for handling all actions related to the <see cref="Match"/> model.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class MatchController : ControllerBase
{
    private readonly IMatchService _matchService;

    /// <summary>
    /// Initializes a new instance of <see cref="MatchController"/>
    /// </summary>
    /// <param name="matchService">Service handling all <see cref="Match"/> actions.</param>
    public MatchController(IMatchService matchService)
    {
        _matchService = matchService;
    }

    /// <summary>
    /// Gets all available matches for a <see cref="Poule"/>
    /// </summary>
    /// <param name="pouleId">The identifier of the existing <see cref="Pouole"/></param>
    /// <returns>List of <see cref="Match"/> of the specified <see cref="Poule"/></returns>
    [HttpGet($"{nameof(GetList)}/{{pouleId}}")]
    public IActionResult GetList([FromRoute]int pouleId) 
    {
        var list = _matchService.GetAll(pouleId);
        if (list == null) {
            return NotFound();
        }
        return Ok(list); 
    }

    /// <summary>
    /// Gets a single <see cref="Match"/> based on its identifier
    /// </summary>
    /// <param name="id">The identifier of an existing <see cref="Match"/></param>
    /// <returns>The requested <see cref="Match"/> if found</returns>
    [HttpGet($"{nameof(GetById)}/{{id}}")]
    public IActionResult GetById([FromRoute]int id) 
    {
        var entity = _matchService.Get(id);
        if (entity == null) {
            return NotFound();
        }
        return Ok(entity);
    }

    /// <summary>
    /// Creates a new <see cref="Match"/> object.
    /// </summary>
    /// <param name="match">The new <see cref="Match"/></param>
    /// <returns>The created <see cref="Match"/></returns>
    [HttpPost($"{nameof(Create)}")]
    public IActionResult Create([FromBody]Match match) 
    {
        if (match == null) {
            return BadRequest("Something went wrong");
        }

        _matchService.Insert(match);
        return Ok(match);
    }

    /// <summary>
    /// Deletes a <see cref="Match"/> based on its identifier
    /// </summary>
    /// <param name="id">The identifier of an existing <see cref="Match"/></param>
    /// <returns>Status code 200 if deletion is successful</returns>
    [HttpDelete($"{nameof(Delete)}/{{id}}")]
    public IActionResult Delete(int id)
    {
        _matchService.Delete(id);
        return Ok();
    }
}
