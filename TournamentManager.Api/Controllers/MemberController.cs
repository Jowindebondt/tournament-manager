using Microsoft.AspNetCore.Mvc;
using TournamentManager.Application;
using TournamentManager.Domain;

namespace TournamentManager.Api;

/// <summary>
/// This controller is responsible for handling all actions related to the <see cref="Member"/> model.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class MemberController : ControllerBase
{
    private readonly IMemberService _memberService;

    /// <summary>
    /// Initializes a new instance of <see cref="MemberController"/>
    /// </summary>
    /// <param name="memberService">Service handling all <see cref="Member"/> actions.</param>
    public MemberController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    /// <summary>
    /// Gets all available members for a <see cref="Tournament"/>
    /// </summary>
    /// <param name="tournamentId">The identifier of the existing <see cref="Tournament"/></param>
    /// <returns>List of <see cref="Member"/> of the specified <see cref="Tournament"/></returns>
    [HttpGet($"{nameof(GetList)}/{{tournamentId}}")]
    public IActionResult GetList([FromRoute]int tournamentId) 
    {
        var list = _memberService.GetAll(tournamentId);
        if (list == null) {
            return NotFound();
        }
        return Ok(list); 
    }

    /// <summary>
    /// Gets a single <see cref="Member"/> based on its identifier
    /// </summary>
    /// <param name="id">The identifier of an existing <see cref="Member"/></param>
    /// <returns>The requested <see cref="Member"/> if found</returns>
    [HttpGet($"{nameof(GetById)}/{{id}}")]
    public IActionResult GetById([FromRoute]int id) 
    {
        var entity = _memberService.Get(id);
        if (entity == null) {
            return NotFound();
        }
        return Ok(entity);
    }

    /// <summary>
    /// Creates a new <see cref="Member"/> object.
    /// </summary>
    /// <param name="member">The new <see cref="Member"/></param>
    /// <returns>The created <see cref="Member"/></returns>
    [HttpPost($"{nameof(Create)}")]
    public IActionResult Create([FromBody]Member member) 
    {
        if (member == null) {
            return BadRequest("Something went wrong");
        }

        _memberService.Insert(member);
        return Ok(member);
    }

    /// <summary>
    /// Updates an existing <see cref="Member"/>
    /// </summary>
    /// <param name="id">Id of the existing <see cref="Member"/></param>
    /// <param name="member">The <see cref="Member"/> object with the desired changes</param>
    /// <returns>The updated <see cref="Member"/></returns>
    [HttpPut($"{nameof(Update)}/{{id}}")]
    public IActionResult Update([FromRoute]int id, [FromBody]Member member) 
    {
        if (member == null){
            return BadRequest("Something went wrong");
        }

        var modified = _memberService.Update(id, member);
        return Ok(modified);
    }

    /// <summary>
    /// Deletes a <see cref="Member"/> based on its identifier
    /// </summary>
    /// <param name="id">The identifier of an existing <see cref="Member"/></param>
    /// <returns>Status code 200 if deletion is successful</returns>
    [HttpDelete($"{nameof(Delete)}/{{id}}")]
    public IActionResult Delete(int id)
    {
        _memberService.Delete(id);
        return Ok();
    }
}
