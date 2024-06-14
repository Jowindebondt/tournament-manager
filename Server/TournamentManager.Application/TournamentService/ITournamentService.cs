using TournamentManager.Domain;

namespace TournamentManager.Application;

/// <summary>
/// Interface defining the service for handling actions for the <see cref="Tournament"/> model.
/// </summary>
public interface ITournamentService
{
    /// <summary>
    /// Deletes an existing <see cref="Tournament"/> instance.
    /// </summary>
    /// <param name="id">The identifier of the <see cref="Tournament"/></param>
    void Delete(int id);
    /// <summary>
    /// Generates all matches and games according to the settings of the tournament and rounds.
    /// </summary>
    /// <param name="id">The identifier of the <see cref="Tournament"/></param>
    void Generate(int id);
    /// <summary>
    /// Gets a single <see cref="Tournament"/> based on its identifier.
    /// </summary>
    /// <param name="id">The identifier of a <see cref="Tournament"/></param>
    /// <returns>The requested <see cref="Tournament"/></returns>
    Tournament Get(int id);
    /// <summary>
    /// Gets a list of available <see cref="Tournament"/>s=.
    /// </summary>
    /// <returns>List of <see cref="Tournament"/>s</returns>
    IEnumerable<Tournament> GetAll();
    /// <summary>
    /// Create a new instance of <see cref="Tournament"/>
    /// </summary>
    /// <param name="entity">The new values for <see cref="Tournament"/></param>
    void Insert(Tournament entity);
    /// <summary>
    /// Sets the settings for the <see cref="Tournament"/>
    /// </summary>
    /// <param name="settings">The settings</param>
    void SetSettings(TournamentSettings settings);
    /// <summary>
    /// Updates an existing <see cref="Tournament"/> instance.
    /// </summary>
    /// <param name="id">The identifier of the <see cref="Tournament"/></param>
    /// <param name="entity">The <see cref="Tournament"/> changes.</param>
    /// <returns>The updated <see cref="Tournament"/></returns>
    Tournament Update(int id, Tournament entity);
}
    