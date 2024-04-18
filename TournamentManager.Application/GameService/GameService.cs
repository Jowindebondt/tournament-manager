using TournamentManager.Domain;

namespace TournamentManager.Application;

public class GameService : IGameService
{
    private readonly IMatchService _matchService;
    private readonly ICrudService<Game> _crudService;

    /// <summary>
    /// Initializes a new instance of <see cref="GameService"/>
    /// </summary>
    /// <param name="matchService">Service handling all <see cref="Match"/> actions.</param>
    /// <param name="crudService">Service for handling CRUD actions for the <see cref="Game"/> model.</param>
    public GameService(IMatchService matchService, ICrudService<Game> crudService)
    {
        _matchService = matchService;
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
        return _crudService.GetAll(u => u.Match.Id == parentId);
    }

    /// <inheritdoc/>
    public void Insert(int parentId, Game entity)
    {
        _crudService.Insert(entity, () => {
            var match = _matchService.Get(parentId) ?? throw new NullReferenceException($"{nameof(Match)} not found"); 
            entity.Match = match;
        });
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
