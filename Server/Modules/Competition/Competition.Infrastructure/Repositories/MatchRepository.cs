using Competition.Domain.Entities;
using Competition.Domain.Interfaces;
using Competition.Domain.ValueObjects;
using Competition.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Competition.Infrastructure.Repositories;

public class MatchRepository : IMatchRepository
{
    private readonly CompetitionDbContext _competitionDbContext;
    private readonly DbSet<Match> _matches;

    public MatchRepository(CompetitionDbContext competitionDbContext)
    {
        _competitionDbContext = competitionDbContext;
        _matches = competitionDbContext.Set<Match>();
    }

    public async Task<ICollection<Match>> GetAllByPouleAsync(PouleId pouleId)
    {
        ArgumentNullException.ThrowIfNull(pouleId, nameof(pouleId));

        return await _matches
            .Where(m => m.PouleId == pouleId)
            .ToListAsync();
    }

    public async Task<Match?> GetByIdAsync(MatchId id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));

        return await _matches.SingleOrDefaultAsync(m => m.Id == id);
    }

    public async Task UpdateAsync(Match match)
    {
        ArgumentNullException.ThrowIfNull(match, nameof(match));

        _matches.Update(match);
        await _competitionDbContext.SaveChangesAsync();
    }
}
