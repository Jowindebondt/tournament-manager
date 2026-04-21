using Design.Domain;
using Design.Domain.Entities;
using Design.Domain.ValueObjects;
using Design.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Design.Infrastructure.Repositories;

public class RoundRepository : IRoundRepository
{
    private readonly DesignDbContext _designDbContext;
    private readonly DbSet<Round> _rounds;

    public RoundRepository(DesignDbContext designDbContext)
    {
        _designDbContext = designDbContext;
        _rounds = designDbContext.Rounds;
    }

    public async Task AddAsync(Round round)
    {
        ArgumentNullException.ThrowIfNull(round, nameof(round));

        _rounds.Add(round);
        await _designDbContext.SaveChangesAsync();
    }

    public async Task<ICollection<Round>> GetAllByTournamentAsync(TournamentId tournamentId)
    {
        ArgumentNullException.ThrowIfNull(tournamentId, nameof(tournamentId));
        
        return await _rounds
            .Include(round => round.Tournament)
            .Where(r => r.Tournament.Id == tournamentId)
            .ToListAsync();
    }

    public async Task<Round> GetByIdAsync(RoundId id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));

        return await _rounds.SingleAsync(r => r.Id == id);
    }

    public async Task RemoveAsync(Round round)
    {
        ArgumentNullException.ThrowIfNull(round, nameof(round));
        
        _rounds.Remove(round);
        await _designDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Round round)
    {
        ArgumentNullException.ThrowIfNull(round, nameof(round));
        
        _rounds.Update(round);
        await _designDbContext.SaveChangesAsync();
    }
}
