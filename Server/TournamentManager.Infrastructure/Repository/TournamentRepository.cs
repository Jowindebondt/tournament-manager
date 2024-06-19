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
    public Tournament GetWithAllRelations(int id)
    {
        return Entities
            .Include(t => t.Settings)
            .Include(t => t.Members)
                .ThenInclude(m => m.Player)
            .Include(t => t.Rounds)
                .ThenInclude(r => r.Settings)
            .Include(t => t.Rounds)
                .ThenInclude(r => r.Poules)
                    .ThenInclude(pp => pp.Players)
            .Include(t => t.Rounds)
                .ThenInclude(r => r.Poules)
                    .ThenInclude(p => p.Matches)
                        .ThenInclude(m => m.Games)
            .Include(t => t.Rounds)
                .ThenInclude(r => r.Poules)
                    .ThenInclude(p => p.Matches)
                        .ThenInclude(m => m.Player1)
            .Include(t => t.Rounds)
                .ThenInclude(r => r.Poules)
                    .ThenInclude(p => p.Matches)
                        .ThenInclude(m => m.Player2)
            .AsSplitQuery()
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
