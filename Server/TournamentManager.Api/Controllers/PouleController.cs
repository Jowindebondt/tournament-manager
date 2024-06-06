using Microsoft.AspNetCore.Mvc;
using TournamentManager.Application;
using TournamentManager.Domain;

namespace TournamentManager.Api;

/// <summary>
/// This controller is responsible for handling all actions related to the <see cref="Poule"/> model.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PouleController : ControllerBase
{
    private readonly IPouleService _pouleService;

    /// <summary>
    /// Initializes a new instance of <see cref="PouleController"/>
    /// </summary>
    /// <param name="pouleService">Service handling all <see cref="Poule"/> actions.</param>
    public PouleController(IPouleService pouleService)
    {
        _pouleService = pouleService;
    }

    /// <summary>
    /// Gets all available poules for a <see cref="Round"/>
    /// </summary>
    /// <param name="roundId">The identifier of the existing <see cref="Round"/></param>
    /// <returns>List of <see cref="Poule"/> of the specified <see cref="Round"/></returns>
    [HttpGet($"{nameof(GetList)}/{{roundId}}")]
    public IActionResult GetList([FromRoute]int roundId) 
    {
        var list = _pouleService.GetAll(roundId);
        if (list == null) {
            return NotFound();
        }
        return Ok(list); 
    }

    /// <summary>
    /// Gets a single <see cref="Poule"/> based on its identifier
    /// </summary>
    /// <param name="id">The identifier of an existing <see cref="Poule"/></param>
    /// <returns>The requested <see cref="Poule"/> if found</returns>
    [HttpGet($"{nameof(GetById)}/{{id}}")]
    public IActionResult GetById([FromRoute]int id) 
    {
        var entity = _pouleService.Get(id);
        if (entity == null) {
            return NotFound();
        }
        return Ok(entity);
    }

    /// <summary>
    /// Creates a new <see cref="Poule"/> object.
    /// </summary>
    /// <param name="roundId">The identifier of an existing <see cref="Round"/></param>
    /// <param name="poule">The new <see cref="Poule"/></param>
    /// <returns>The created <see cref="Poule"/></returns>
    [HttpPost($"{nameof(Create)}")]
    public IActionResult Create([FromBody]Poule poule) 
    {
        if (poule == null) {
            return BadRequest("Something went wrong");
        }

        _pouleService.Insert(poule);
        return Ok(poule);
    }

    /// <summary>
    /// Updates an existing <see cref="Poule"/>
    /// </summary>
    /// <param name="id">Id of the existing <see cref="Poule"/></param>
    /// <param name="poule">The <see cref="Poule"/> object with the desired changes</param>
    /// <returns>The updated <see cref="Poule"/></returns>
    [HttpPut($"{nameof(Update)}/{{id}}")]
    public IActionResult Update([FromRoute]int id, [FromBody]Poule poule) 
    {
        if (poule == null){
            return BadRequest("Something went wrong");
        }

        var modified = _pouleService.Update(id, poule);
        return Ok(modified);
    }

    /// <summary>
    /// Deletes a <see cref="Poule"/> based on its identifier
    /// </summary>
    /// <param name="id">The identifier of an existing <see cref="Poule"/></param>
    /// <returns>Status code 200 if deletion is successful</returns>
    [HttpDelete($"{nameof(Delete)}/{{id}}")]
    public IActionResult Delete(int id)
    {
        _pouleService.Delete(id);
        return Ok();
    }

    [HttpPost($"{nameof(AddMembers)}/{{id}}")]
    public IActionResult AddMembers([FromRoute]int id, [FromBody]IEnumerable<int> memberIds)
    {
        if (memberIds == null || !memberIds.Any())
        {
            return BadRequest("Something went wrong");
        }

        _pouleService.AddMembers(id, memberIds);
        return Ok();
    }

    [HttpPost($"{nameof(AddMembersAsTeam)}/{{id}}")]
    public IActionResult AddMembersAsTeam([FromRoute]int id, [FromBody]IEnumerable<int> memberIds)
    {
        if (memberIds == null || !memberIds.Any())
        {
            return BadRequest("Something went wrong");
        }

        _pouleService.AddMembersAsTeam(id, memberIds);
        return Ok();
    }
}
