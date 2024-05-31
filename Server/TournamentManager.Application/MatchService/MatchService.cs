using TournamentManager.Domain;

namespace TournamentManager.Application;

public class MatchService : IMatchService
{
    private readonly ICrudService<Match> _crudService;

    public MatchService(ICrudService<Match> crudService)
    {
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
    public void Insert(Match entity)
    {
        _crudService.Insert(entity);
    }
}
