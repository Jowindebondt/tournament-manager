using TournamentManager.Application.Repositories;
using TournamentManager.Domain;

namespace TournamentManager.Application;

/// <summary>
/// Implementing class of the <see cref="IRoundService"/> interface
/// </summary>
public class RoundService : IRoundService
{
    private readonly ITournamentService _tournamentService;
    private readonly ICrudService<Round> _crudService;

    /// <summary>
    /// Initializes a new instance of <see cref="RoundService"/>
    /// </summary>
    /// <param name="tournamentService">Service handling all <see cref="Tournament"/> actions.</param>
    /// <param name="crudService">Service for handling CRUD actions for the <see cref="Round"/> model.</param>
    public RoundService(ITournamentService tournamentService, ICrudService<Round> crudService)
    {
        _tournamentService = tournamentService;
        _crudService = crudService;
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        _crudService.Delete(id);
    }

    /// <inheritdoc/>
    public Round Get(int id)
    {
        return _crudService.Get(id);
    }

    /// <inheritdoc/>
    public IEnumerable<Round> GetAll(int parentId)
    {
        return _crudService.GetAll(u => u.Tournament.Id == parentId);
    }

    /// <inheritdoc/>
    public void Insert(int parentId, Round entity)
    {
        _crudService.Insert(entity, () => {
            var tournament = _tournamentService.Get(parentId) ?? throw new NullReferenceException($"{nameof(Tournament)} not found"); 
            entity.Tournament = tournament;
        });
    }

    /// <inheritdoc/>
    public Round Update(int id, Round entity)
    {
        return _crudService.Update(id, entity, (origin) => {
            origin.Name = entity.Name;
        });
    }
}
