using Microsoft.EntityFrameworkCore;
using TournamentManager.Application;
using TournamentManager.Domain;

namespace TournamentManager.Infrastructure;

/// <summary>
/// Implementing class of the <see cref="ITournamentRepository"/> interface.
/// </summary>
public class TournamentRepository : Repository<Tournament>, ITournamentRepository
{
    /// <summary>
    /// Initializes a new instance of <see cref="TournamentRepository"/>
    /// </summary>
    /// <param name="applicationDbContext">The database context</param>
    public TournamentRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }

    /// <inheritdoc/>
    public Tournament GetWithAllReferences(int id)
    {
        return Entities
            .Include(u => u.Settings)
            .Include(u => u.Members)
            .Include(u => u.Rounds)
                .ThenInclude(u => u.Poules)
                    .ThenInclude(u => u.Matches)
                        .ThenInclude(u => u.Games)
            .FirstOrDefault(u => u.Id == id);
    }

    /// <inheritdoc/>
    public Tournament GetWithSettings(int id)
    {
        return Entities
            .Include(u => u.Settings)
            .FirstOrDefault(u => u.Id == id);
    }
}
