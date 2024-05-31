using TournamentManager.Domain;

namespace TournamentManager.Application;

public interface IPlayerService
{
    /// <summary>
    /// Deletes an existing <see cref="Player"/> instance.
    /// </summary>
    /// <param name="id">The identifier of the <see cref="Player"/></param>
    void Delete(int id);
    /// <summary>
    /// Gets a single <see cref="Player"/> based on its identifier.
    /// </summary>
    /// <param name="id">The identifier of a <see cref="Player"/></param>
    /// <returns>The requested <see cref="Player"/></returns>
    Player Get(int id);
    /// <summary>
    /// Create a new instance of <see cref="Player"/>
    /// </summary>
    /// <param name="entity">The new values for <see cref="Player"/></param>
    void Insert(Player entity);
}
