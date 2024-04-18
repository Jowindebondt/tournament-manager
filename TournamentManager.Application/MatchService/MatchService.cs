using TournamentManager.Domain;

namespace TournamentManager.Application;

public class MatchService : IMatchService
{
    private readonly IPouleService _pouleService;
    private readonly ICrudService<Match> _crudService;

    public MatchService(IPouleService pouleService, ICrudService<Match> crudService)
    {
        _pouleService = pouleService;
        _crudService = crudService;
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        _crudService.Delete(id);
    }

    /// <inheritdoc/>
    public Match Get(int id)
    {
        return _crudService.Get(id);
    }

    /// <inheritdoc/>
    public IEnumerable<Match> GetAll(int parentId)
    {
        return _crudService.GetAll(u => u.Poule.Id == parentId);
    }

    /// <inheritdoc/>
    public void Insert(int parentId, Match entity)
    {
        _crudService.Insert(entity, () => {
            var poule = _pouleService.Get(parentId) ?? throw new NullReferenceException($"{nameof(Round)} not found"); 
            entity.Poule = poule;
        });
    }
}
