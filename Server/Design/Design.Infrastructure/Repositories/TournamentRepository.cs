using Design.Domain;
using Design.Domain.Entities;
using Design.Domain.ValueObjects;
using Design.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Design.Infrastructure.Repositories;

public class TournamentRepository : ITournamentRepository
{
    private readonly DesignDbContext _designDbContext;
    private readonly DbSet<Tournament> _tournaments;

    public TournamentRepository(DesignDbContext designDbContext)
    {
        _designDbContext = designDbContext;
        _tournaments = designDbContext.Tournaments;
    }

    public async Task AddAsync(Tournament tournament)
    {
        ArgumentNullException.ThrowIfNull(tournament, nameof(tournament));

        await _tournaments.AddAsync(tournament);
        await _designDbContext.SaveChangesAsync();
    }

    public async Task<ICollection<Tournament>> GetAllAsync()
    {
        return await _tournaments.ToListAsync();
    }

    public async Task<Tournament?> GetByIdAsync(TournamentId id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));

        return await _tournaments.FindAsync(id);
    }

    public async Task RemoveAsync(Tournament tournament)
    {
        ArgumentNullException.ThrowIfNull(tournament, nameof(tournament));
        
        _tournaments.Remove(tournament);
        await _designDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tournament tournament)
    {
        ArgumentNullException.ThrowIfNull(tournament, nameof(tournament));
        
        _tournaments.Update(tournament);
        await _designDbContext.SaveChangesAsync();
    }
}
