using TournamentManager.Application.Repositories;
using TournamentManager.Domain;

namespace TournamentManager.Application;

public class PouleService : IPouleService
{
    private readonly ICrudService<Poule> _crudService;

    /// <summary>
    /// Initializes a new instance of <see cref="PouleService"/>
    /// </summary>
    /// <param name="crudService">Service for handling CRUD actions for the <see cref="Poule"/> model.</param>
    public PouleService(ICrudService<Poule> crudService) 
    {
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
        return _crudService.GetAll(u => u.RoundId == parentId);
    }

    /// <inheritdoc/>
    public void Insert(Poule entity)
    {
        _crudService.Insert(entity);
    }

    /// <inheritdoc/>
    public Poule Update(int id, Poule entity)
    {
        return _crudService.Update(id, entity, (origin) => {
            origin.Name = entity.Name;
        });
    }
}
