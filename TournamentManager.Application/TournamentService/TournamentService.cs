using TournamentManager.Application.Repositories;
using TournamentManager.Domain;

namespace TournamentManager.Application;

/// <summary>
/// Implementing class of the <see cref="ITournamentService"/> interface
/// </summary>
public class TournamentService : ITournamentService
{
    private readonly ICrudService<Tournament> _crudService;

    /// <summary>
    /// Initializes a new instance of <see cref="TournamentService"/>
    /// </summary>
    /// <param name="crudService">Service for handling CRUD actions for the <see cref="Tournament"/> model.</param>
    public TournamentService(ICrudService<Tournament> crudService){
        _crudService = crudService;
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        _crudService.Delete(id);
    }

    /// <inheritdoc/>
    public Tournament Get(int id)
    {
        return _crudService.Get(id);
    }

    /// <inheritdoc/>
    public IEnumerable<Tournament> GetAll()
    {
        return _crudService.GetAll();
    }

    /// <inheritdoc/>
    public void Insert(Tournament entity)
    {
        _crudService.Insert(entity);
    }

    /// <inheritdoc/>
    public Tournament Update(int id, Tournament entity)
    {
        return _crudService.Update(id, entity, origin => {
            origin.Name = entity.Name;
        });
    }
}
