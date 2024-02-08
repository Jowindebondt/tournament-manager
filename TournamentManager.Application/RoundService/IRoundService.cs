using TournamentManager.Domain;

namespace TournamentManager.Application;

/// <summary>
/// Interface defining the service for handling actions for the <see cref="Round"/> model.
/// </summary>
public interface IRoundService
{
    /// <summary>
    /// Deletes an existing <see cref="Round"/> instance.
    /// </summary>
    /// <param name="id">The identifier of the <see cref="Round"/></param>
    void Delete(int id);
    /// <summary>
    /// Gets a single <see cref="Round"/> based on its identifier.
    /// </summary>
    /// <param name="id">The identifier of a <see cref="Round"/></param>
    /// <returns>The requested <see cref="Tournament"/></returns>
    Round Get(int id);
    /// <summary>
    /// Gets a list of available <see cref="Round"/>s for the specified <see cref="Tournament"/>.
    /// </summary>
    /// <param name="tournamentId">Identifier of the <see cref="Tournament"/></param>
    /// <returns>List of <see cref="Round"/>s for the specified <see cref="Tournament"/></returns>
    IEnumerable<Round> GetAll(int tournamentId);
    /// <summary>
    /// Creates a new instance of the <see cref="Round"/> model as part of the specified <see cref="Tournament"/>.
    /// </summary>
    /// <param name="tournamentId">Identifier of the <see cref="Tournament"/></param>
    /// <param name="round">The <see cref="Round"/> to create.</param>
    void Insert(int tournamentId, Round round);
    /// <summary>
    /// Updates an existing <see cref="Round"/> instance.
    /// </summary>
    /// <param name="id">The identifier of the <see cref="Round"/></param>
    /// <param name="round">The <see cref="Round"/> changes.</param>
    /// <returns></returns>
    Round Update(int id, Round round);
}
