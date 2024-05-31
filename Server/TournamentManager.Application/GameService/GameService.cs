using TournamentManager.Domain;

namespace TournamentManager.Application;

public class GameService : IGameService
{
    private readonly ICrudService<Game> _crudService;

    /// <summary>
    /// Initializes a new instance of <see cref="GameService"/>
    /// </summary>
    /// <param name="crudService">Service for handling CRUD actions for the <see cref="Game"/> model.</param>
    public GameService(ICrudService<Game> crudService)
    {
        _crudService = crudService;
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        _crudService.Delete(id);
    }

    /// <inheritdoc/>
    public Game Get(int id)
    {
        return _crudService.Get(id);
    }

    /// <inheritdoc/>
    public IEnumerable<Game> GetAll(int parentId)
    {
        return _crudService.GetAll(u => u.MatchId == parentId);
    }

    /// <inheritdoc/>
    public void Insert(Game entity)
    {
        _crudService.Insert(entity);
    }

    /// <inheritdoc/>
    public Game Update(int id, Game entity)
    {
        return _crudService.Update(id, entity, (origin) => {
            origin.Score_1 = entity.Score_1;
            origin.Score_2 = entity.Score_2;
        });
    }
}
