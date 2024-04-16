using Microsoft.AspNetCore.Mvc;
using TournamentManager.Application;
using TournamentManager.Domain;

namespace TournamentManager.Api;

/// <summary>
/// This controller is responsible for handling all actions related to the <see cref="PouleMember"/> model.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PouleMemberController : ControllerBase
{
    private readonly IPouleMemberService _pouleMemberService;

    /// <summary>
    /// Initializes a new instance of <see cref="PouleMemberController"/>
    /// </summary>
    /// <param name="pouleMemberService">Service handling all <see cref="PouleMember"/> actions.</param>
    public PouleMemberController(IPouleMemberService pouleMemberService)
    {
        _pouleMemberService = pouleMemberService;
    }

    /// <summary>
    /// Gets all available members for a <see cref="Poule"/>
    /// </summary>
    /// <param name="pouleId">The identifier of the existing <see cref="Poule"/></param>
    /// <returns>List of <see cref="PouleMember"/> of the specified <see cref="Poule"/></returns>
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
    /// Creates a new <see cref="PouleMember"/> object.
    /// </summary>
    /// <param name="pouleId">The identifier of an existing <see cref="Poule"/></param>
    /// <param name="memberId">The identifier of an existing <see cref="Member"/></param>
    /// <returns>The created <see cref="PouleMember"/></returns>
    [HttpPost($"{nameof(Create)}/{{pouleId}}/{{memberId}}")]
    public IActionResult Create([FromRoute]int pouleId, [FromRoute]int memberId) 
    {
        var pouleMember = _pouleMemberService.Create(pouleId, memberId);
        return Ok(pouleMember);
    }

    /// <summary>
    /// Deletes a <see cref="PouleMember"/> based on its identifier
    /// </summary>
    /// <param name="id">The identifier of an existing <see cref="PouleMember"/></param>
    /// <returns>Status code 200 if deletion is successful</returns>
    [HttpDelete($"{nameof(Delete)}/{{id}}")]
    public IActionResult Delete(int id)
    {
        _pouleMemberService.Delete(id);
        return Ok();
    }
}
