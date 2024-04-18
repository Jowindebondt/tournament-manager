using TournamentManager.Domain;

namespace TournamentManager.Application;

/// <summary>
/// Interface defining the service for handling actions for the <see cref="Match"/> model.
/// </summary>
public interface IMatchService
{
    /// <summary>
    /// Deletes an existing <see cref="Match"/> instance.
    /// </summary>
    /// <param name="id">The identifier of the <see cref="Match"/></param>
    void Delete(int id);
    /// <summary>
    /// Gets a single <see cref="Match"/> based on its identifier.
    /// </summary>
    /// <param name="id">The identifier of a <see cref="Match"/></param>
    /// <returns>The requested <see cref="Match"/></returns>
    Match Get(int id);
    /// <summary>
    /// Gets a list of available <see cref="Match"/>es for the specified <see cref="Poule"/>.
    /// </summary>
    /// <param name="parentId">Id of the <see cref="Poule"/></param>
    /// <returns>List of <see cref="Match"/>es for the specified <see cref="Poule"/></returns>
    IEnumerable<Match> GetAll(int parentId);
    /// <summary>
    /// Create a new instance of <see cref="Match"/>
    /// </summary>
    /// <param name="parentId">Id of the <see cref="Poule"/></param>
    /// <param name="entity">The new values for <see cref="Match"/></param>
    void Insert(int parentId, Match entity);
}
