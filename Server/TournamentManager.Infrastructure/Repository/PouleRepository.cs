using Microsoft.EntityFrameworkCore;
using TournamentManager.Application;
using TournamentManager.Domain;

namespace TournamentManager.Infrastructure;

/// <summary>
/// Implementing class of the <see cref="PouleRepository"/> interface.
/// </summary>
public class PouleRepository : Repository<Poule>, IPouleRepository
{
    /// <summary>
    /// Initializes a new instance of <see cref="PouleRepository"/>
    /// </summary>
    /// <param name="applicationDbContext">The database context</param>
    public PouleRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }

    /// <inheritdoc/>
    public Poule GetWithAncestors(int id)
    {
        return Entities
            .Include(u => u.Round)
                .ThenInclude(u => u.Tournament)
            .SingleOrDefault(u => u.Id == id);
    }
}
