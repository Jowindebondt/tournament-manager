using Microsoft.AspNetCore.Mvc;
using TournamentManager.Application;
using TournamentManager.Domain;

namespace TournamentManager.Api;

[Route("api/[controller]")]
[ApiController]
public class TournamentController : ControllerBase
{
    private readonly ITournamentService _tournamentService;

    public TournamentController(ITournamentService tournamentService) {
        _tournamentService = tournamentService;
    }

    [HttpGet(nameof(GetList))]
    public IActionResult GetList() {
        var list = _tournamentService.GetAll();
        if (list == null) {
            return NotFound();
        }
        return Ok(list); 
    }

    [HttpGet(nameof(GetById))]
    public IActionResult GetById(int id) {
        var entity = _tournamentService.Get(id);
        if (entity == null) {
            return NotFound();
        }
        return Ok(entity);
    }

    [HttpPost(nameof(Create))]
    public IActionResult Create(Tournament tournament) {
        if (tournament == null) {
            return BadRequest("Something went wrong");
        }

        _tournamentService.Insert(tournament);
        return Ok(tournament);
    }
}
