using Microsoft.AspNetCore.Mvc;
using TournamentManager.Application;
using TournamentManager.Domain;

namespace TournamentManager.Api;

/// <summary>
/// This controller is responsible for handling all actions related to the <see cref="PoulePlayer"/> model.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PouleMemberController : ControllerBase
{
    private readonly IPoulePlayerService _pouleMemberService;

    /// <summary>
    /// Initializes a new instance of <see cref="PouleMemberController"/>
    /// </summary>
    /// <param name="pouleMemberService">Service handling all <see cref="PoulePlayer"/> actions.</param>
    public PouleMemberController(IPoulePlayerService pouleMemberService)
    {
        _pouleMemberService = pouleMemberService;
    }

    /// <summary>
    /// Gets all available members for a <see cref="Poule"/>
    /// </summary>
    /// <param name="pouleId">The identifier of the existing <see cref="Poule"/></param>
    /// <returns>List of <see cref="PoulePlayer"/> of the specified <see cref="Poule"/></returns>
    [HttpGet($"{nameof(GetList)}/{{pouleId}}")]
    public IActionResult GetList([FromRoute]int pouleId) 
    {
        var list = _pouleMemberService.GetAll(pouleId);
        if (list == null) {
            return NotFound();
        }
        return Ok(list); 
    }

    /// <summary>
    /// Creates a new <see cref="PoulePlayer"/> object.
    /// </summary>
    /// <param name="pouleId">The identifier of an existing <see cref="Poule"/></param>
    /// <param name="memberId">The identifier of an existing <see cref="Member"/></param>
    /// <returns>The created <see cref="PoulePlayer"/></returns>
    [HttpPost($"{nameof(Create)}/{{pouleId}}/{{memberId}}")]
    public IActionResult Create([FromRoute]int pouleId, [FromRoute]int memberId) 
    {
        var pouleMember = _pouleMemberService.Create(pouleId, memberId);
        return Ok(pouleMember);
    }

    /// <summary>
    /// Deletes a <see cref="PoulePlayer"/> based on its identifier
    /// </summary>
    /// <param name="id">The identifier of an existing <see cref="PoulePlayer"/></param>
    /// <returns>Status code 200 if deletion is successful</returns>
    [HttpDelete($"{nameof(Delete)}/{{id}}")]
    public IActionResult Delete(int id)
    {
        _pouleMemberService.Delete(id);
        return Ok();
    }
}
