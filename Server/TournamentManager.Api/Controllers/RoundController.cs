using Microsoft.AspNetCore.Mvc;
using TournamentManager.Application;
using TournamentManager.Domain;

namespace TournamentManager.Api;

/// <summary>
/// This controller is responsible for handling all actions related to the <see cref="Round"/> model.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class RoundController : ControllerBase
{
    private readonly IRoundService _roundService;
    
    /// <summary>
    /// Initializes a new instance of <see cref="RoundController"/>
    /// </summary>
    /// <param name="roundService">Service handling all <see cref="Round"/> actions.</param>
    public RoundController(IRoundService roundService)
    {
        _roundService = roundService;
    }

    /// <summary>
    /// Gets all available rounds for a <see cref="Tournament"/>
    /// </summary>
    /// <returns>List of <see cref="Round"/> of the specified <see cref="Tournament"/></returns>
    [HttpGet($"{nameof(GetList)}/{{tournamentId}}")]
    public IActionResult GetList([FromRoute]int tournamentId) 
    {
        var list = _roundService.GetAll(tournamentId);
        return Ok(list); 
    }

    /// <summary>
    /// Gets a single <see cref="Round"/> based on its identifier
    /// </summary>
    /// <param name="id">The identifier of an existing <see cref="Round"/></param>
    /// <returns>The requested <see cref="Round"/> if found</returns>
    [HttpGet($"{nameof(GetById)}/{{id}}")]
    public IActionResult GetById([FromRoute]int id) 
    {
        var entity = _roundService.Get(id);
        if (entity == null) {
            return NotFound();
        }
        return Ok(entity);
    }

    /// <summary>
    /// Creates a new <see cref="Round"/> object.
    /// </summary>
    /// <param name="round">The new <see cref="Round"/></param>
    /// <returns>The created <see cref="Round"/></returns>
    [HttpPost($"{nameof(Create)}")]
    public IActionResult Create([FromBody]Round round) 
    {
        if (round == null) {
            return BadRequest("Something went wrong");
        }

        _roundService.Insert(round);
        return Ok(round);
    }

    /// <summary>
    /// Updates an existing <see cref="Round"/>
    /// </summary>
    /// <param name="id">Id of the existing <see cref="Round"/></param>
    /// <param name="round">The <see cref="Round"/> object with the desired changes</param>
    /// <returns>The updated <see cref="Round"/></returns>
    [HttpPut($"{nameof(Update)}/{{id}}")]
    public IActionResult Update([FromRoute]int id, [FromBody]Round round) 
    {
        if (round == null){
            return BadRequest("Something went wrong");
        }

        var modified = _roundService.Update(id, round);
        return Ok(modified);
    }

    /// <summary>
    /// Deletes a <see cref="Round"/> based on its identifier
    /// </summary>
    /// <param name="id">The identifier of an existing <see cref="Round"/></param>
    /// <returns>Status code 200 if deletion is successful</returns>
    [HttpDelete($"{nameof(Delete)}/{{id}}")]
    public IActionResult Delete(int id)
    {
        _roundService.Delete(id);
        return Ok();
    }
}
