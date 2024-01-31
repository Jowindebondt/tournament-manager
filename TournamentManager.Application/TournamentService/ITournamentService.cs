using TournamentManager.Domain;

namespace TournamentManager.Application;

/// <summary>
/// Interface defining the service for handling actions for the <see cref="Tournament"/> model.
/// </summary>
public interface ITournamentService
{
    /// <summary>
    /// Gets a list of available <see cref="Tournament"/>s.
    /// </summary>
    /// <returns>List of <see cref="Tournament"/></returns>
    IEnumerable<Tournament> GetAll();
    /// <summary>
    /// Gets a single <see cref="Tournament"/> based on its identifier.
    /// </summary>
    /// <param name="id">The identifier of a <see cref="Tournament"/></param>
    /// <returns>The requested <see cref="Tournament"/></returns>
    Tournament Get(int id);
    /// <summary>
    /// Creates a new instance of the <see cref="Tournament"/> model.
    /// </summary>
    /// <param name="tournament">The <see cref="Tournament"/> to create.</param>
    void Insert(Tournament tournament);
    /// <summary>
    /// Updates an existing <see cref="Tournament"/> instance.
    /// </summary>
    /// <param name="id">The identifier of the <see cref="Tournament"/></param>
    /// <param name="tournament">The <see cref="Tournament"/> changes.</param>
    /// <returns>The updated <see cref="Tournament"/></returns>
    Tournament Update(int id, Tournament tournament);
    /// <summary>
    /// Deletes an existing <see cref="Tournament"/> instance.
    /// </summary>
    /// <param name="id">The identifier of the <see cref="Tournament"/></param>
    void Delete(int id);
}
    