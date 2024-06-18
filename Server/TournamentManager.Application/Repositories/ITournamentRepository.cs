using TournamentManager.Application.Repositories;
using TournamentManager.Domain;

namespace TournamentManager.Application;

/// <summary>
/// Interface defining the <see cref="Tournament"/> actions available for the datasource
/// </summary>
public interface ITournamentRepository : IRepository<Tournament>
{
    /// <summary>
    /// Gets a single instance from the datasource with all (child-)references loaded.
    /// </summary>
    /// <param name="id">The identifier of the instance</param>
    /// <returns>The requested instance of <see cref="T"/></returns>
    Tournament GetWithAllReferences(int id);
    /// <summary>
    /// Gets a single instance from the datasource with the <see cref="Tournament.Settings"/> reference loaded.
    /// </summary>
    /// <param name="id">The identifier of the instance</param>
    /// <returns>The requested instance of <see cref="T"/></returns>
    Tournament GetWithSettings(int id);
}
