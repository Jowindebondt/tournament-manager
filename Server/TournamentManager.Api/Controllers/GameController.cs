using Microsoft.AspNetCore.Mvc;
using TournamentManager.Application;
using TournamentManager.Domain;

namespace TournamentManager.Api;

/// <summary>
/// This controller is responsible for handling all actions related to the <see cref="Game"/> model.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;

    /// <summary>
    /// Initializes a new instance of <see cref="GameController"/>
    /// </summary>
    /// <param name="gameService">Service handling all <see cref="Game"/> actions.</param>
    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    /// <summary>
    /// Gets all available poules for a <see cref="Game"/>
    /// </summary>
    /// <param name="matchId">The identifier of the existing <see cref="Match"/></param>
    /// <returns>List of <see cref="Game"/> of the specified <see cref="Match"/></returns>
    [HttpGet($"{nameof(GetList)}/{{matchId}}")]
    public IActionResult GetList([FromRoute]int matchId) 
    {
        var list = _gameService.GetAll(matchId);
        if (list == null) {
            return NotFound();
        }
        return Ok(list); 
    }

    /// <summary>
    /// Gets a single <see cref="Game"/> based on its identifier
    /// </summary>
    /// <param name="id">The identifier of an existing <see cref="Game"/></param>
    /// <returns>The requested <see cref="Game"/> if found</returns>
    [HttpGet($"{nameof(GetById)}/{{id}}")]
    public IActionResult GetById([FromRoute]int id) 
    {
        var entity = _gameService.Get(id);
        if (entity == null) {
            return NotFound();
        }
        return Ok(entity);
    }

    /// <summary>
    /// Creates a new <see cref="Game"/> object.
    /// </summary>
    /// <param name="game">The new <see cref="Game"/></param>
    /// <returns>The created <see cref="Game"/></returns>
    [HttpPost($"{nameof(Create)}")]
    public IActionResult Create([FromBody]Game game) 
    {
        if (game == null) {
            return BadRequest("Something went wrong");
        }

        _gameService.Insert(game);
        return Ok(game);
    }

    /// <summary>
    /// Updates an existing <see cref="Game"/>
    /// </summary>
    /// <param name="id">Id of the existing <see cref="Game"/></param>
    /// <param name="game">The <see cref="Game"/> object with the desired changes</param>
    /// <returns>The updated <see cref="Game"/></returns>
    [HttpPut($"{nameof(Update)}/{{id}}")]
    public IActionResult Update([FromRoute]int id, [FromBody]Game game) 
    {
        if (game == null){
            return BadRequest("Something went wrong");
        }

        var modified = _gameService.Update(id, game);
        return Ok(modified);
    }

    /// <summary>
    /// Deletes a <see cref="Game"/> based on its identifier
    /// </summary>
    /// <param name="id">The identifier of an existing <see cref="Game"/></param>
    /// <returns>Status code 200 if deletion is successful</returns>
    [HttpDelete($"{nameof(Delete)}/{{id}}")]
    public IActionResult Delete(int id)
    {
        _gameService.Delete(id);
        return Ok();
    }
}
