using TournamentManager.Domain;

namespace TournamentManager.Application;

/// <summary>
/// Interface defining the service for handling actions for the <see cref="Game"/> model.
/// </summary>
public interface IGameService
{
    /// <summary>
    /// Deletes an existing <see cref="Game"/> instance.
    /// </summary>
    /// <param name="id">The identifier of the <see cref="Game"/></param>
    void Delete(int id);
    /// <summary>
    /// Gets a single <see cref="Game"/> based on its identifier.
    /// </summary>
    /// <param name="id">The identifier of a <see cref="Game"/></param>
    /// <returns>The requested <see cref="Game"/></returns>
    Game Get(int id);
    /// <summary>
    /// Gets a list of available <see cref="Game"/>s for the specified <see cref="Match"/>.
    /// </summary>
    /// <param name="parentId">Id of the <see cref="Match"/></param>
    /// <returns>List of <see cref="Game"/>s for the specified <see cref="Match"/></returns>
    IEnumerable<Game> GetAll(int parentId);
    /// <summary>
    /// Create a new instance of <see cref="Game"/>
    /// </summary>
    /// <param name="parentId">Id of the <see cref="Match"/></param>
    /// <param name="entity">The new values for <see cref="Game"/></param>
    void Insert(int parentId, Game entity);
    /// <summary>
    /// Updates an existing <see cref="Game"/> instance.
    /// </summary>
    /// <param name="id">The identifier of the <see cref="Game"/></param>
    /// <param name="entity">The <see cref="Game"/> changes.</param>
    /// <returns>The updated <see cref="Game"/></returns>
    Game Update(int id, Game entity);
}
