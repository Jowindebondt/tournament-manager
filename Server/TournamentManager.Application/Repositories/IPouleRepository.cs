using TournamentManager.Application.Repositories;
using TournamentManager.Domain;

namespace TournamentManager.Application;

public interface IPouleRepository : IRepository<Poule>
{
    /// <summary>
    /// Gets a single instance with all ancestor relations loaded
    /// </summary>
    /// <param name="id">The identifier of the instance</param>
    /// <returns>The requested instance of <see cref="Poule"/></returns>
    Poule GetWithAncestors(int id);
}
