using TournamentManager.Domain;

namespace TournamentManager.Application;

/// <summary>
/// Implementing class of the <see cref="IPlayerService"/> interface
/// </summary>
public class PlayerService : IPlayerService
{
    private readonly ICrudService<Player> _crudService;

    /// <summary>
    /// Initializes a new instance of <see cref="PlayerService"/>
    /// </summary>
    /// <param name="crudService">Service for handling CRUD actions for the <see cref="Player"/> model.</param>
    public PlayerService(ICrudService<Player> crudService)
    {
        _crudService = crudService;
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        _crudService.Delete(id);
    }

    /// <inheritdoc/>
    public Player Get(int id)
    {
        return _crudService.Get(id);
    }

    /// <inheritdoc/>
    public void Insert(Player entity)
    {
        _crudService.Insert(entity);
    }
}
