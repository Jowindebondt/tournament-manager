using TournamentManager.Application.Repositories;
using TournamentManager.Domain;

namespace TournamentManager.Application;

public class PouleService : IPouleService
{
    private readonly IRoundService _roundService;
    private readonly ICrudService<Poule> _crudService;

    /// <summary>
    /// Initializes a new instance of <see cref="PouleService"/>
    /// </summary>
    /// <param name="roundService">Service handling all <see cref="Round"/> actions.</param>
    /// <param name="crudService">Service for handling CRUD actions for the <see cref="Poule"/> model.</param>
    public PouleService(IRoundService roundService, ICrudService<Poule> crudService) 
    {
        _roundService = roundService;
        _crudService = crudService;
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        _crudService.Delete(id);
    }

    /// <inheritdoc/>
    public Poule Get(int id)
    {
        return _crudService.Get(id);
    }

    /// <inheritdoc/>
    public IEnumerable<Poule> GetAll(int parentId)
    {
        return _crudService.GetAll(u => u.Round.Id == parentId);
    }

    /// <inheritdoc/>
    public void Insert(int parentId, Poule entity)
    {
        _crudService.Insert(entity, () => {
            var round = _roundService.Get(parentId) ?? throw new NullReferenceException($"{nameof(Round)} not found"); 
            entity.Round = round;
        });
    }

    /// <inheritdoc/>
    public Poule Update(int id, Poule entity)
    {
        return _crudService.Update(id, entity, (origin) => {
            origin.Name = entity.Name;
        });
    }
}
