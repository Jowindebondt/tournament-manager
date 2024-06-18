using Microsoft.AspNetCore.Mvc;
using TournamentManager.Application;
using TournamentManager.Domain;

namespace TournamentManager.Api;

/// <summary>
/// This controller is responsible for handling all actions related to the <see cref="Tournament"/> model.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TournamentController : ControllerBase
{
    private readonly ITournamentService _tournamentService;

    /// <summary>
    /// Initializes a new instance of <see cref="TournamentController"/>
    /// </summary>
    /// <param name="tournamentService">Service handling all <see cref="Tournament"/> actions.</param>
    public TournamentController(ITournamentService tournamentService) 
    {
        _tournamentService = tournamentService;
    }

    /// <summary>
    /// Gets all available tournaments
    /// </summary>
    /// <returns>List of <see cref="Tournament"/></returns>
    [HttpGet(nameof(GetList))]
    public IActionResult GetList() 
    {
        var list = _tournamentService.GetAll();
        return Ok(list); 
    }

    /// <summary>
    /// Gets a single <see cref="Tournament"/> based on its identifier
    /// </summary>
    /// <param name="id">The identifier of an existing <see cref="Tournament"/></param>
    /// <returns>The requested <see cref="Tournament"/> if found</returns>
    [HttpGet($"{nameof(GetById)}/{{id}}")]
    public IActionResult GetById([FromRoute]int id) 
    {
        var entity = _tournamentService.Get(id);
        if (entity == null) {
            return NotFound();
        }
        return Ok(entity);
    }

    /// <summary>
    /// Creates a new <see cref="Tournament"/> object.
    /// </summary>
    /// <param name="tournament">The new <see cref="Tournament"/></param>
    /// <returns>The created <see cref="Tournament"/></returns>
    [HttpPost(nameof(Create))]
    public IActionResult Create([FromBody]Tournament tournament) 
    {
        if (tournament == null) {
            return BadRequest("Something went wrong");
        }

        _tournamentService.Insert(tournament);
        return Ok(tournament);
    }

    /// <summary>
    /// Updates an existing <see cref="Tournament"/>
    /// </summary>
    /// <param name="id">Id of the existing <see cref="Tournament"/></param>
    /// <param name="tournament">The <see cref="Tournament"/> object with the desired changes</param>
    /// <returns>The updated <see cref="Tournament"/></returns>
    [HttpPut($"{nameof(Update)}/{{id}}")]
    public IActionResult Update([FromRoute]int id, [FromBody]Tournament tournament) 
    {
        if (tournament == null){
            return BadRequest("Something went wrong");
        }

        var modified = _tournamentService.Update(id, tournament);
        return Ok(modified);
    }

    /// <summary>
    /// Deletes a <see cref="Tournament"/> based on its identifier
    /// </summary>
    /// <param name="id">The identifier of an existing <see cref="Tournament"/></param>
    /// <returns>Status code 200 if deletion is successful</returns>
    [HttpDelete($"{nameof(Delete)}/{{id}}")]
    public IActionResult Delete(int id)
    {
        _tournamentService.Delete(id);
        return Ok();
    }

    [HttpPost($"{nameof(Generate)}/{{id}}")]
    public IActionResult Generate([FromRoute]int id)
    {
        _tournamentService.Generate(id);
        return Ok();
    }
}
