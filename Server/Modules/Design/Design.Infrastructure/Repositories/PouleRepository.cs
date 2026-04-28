using Design.Domain.Entities;
using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using Design.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Design.Infrastructure.Repositories;

public class PouleRepository : IPouleRepository
{
    private readonly DesignDbContext _designDbContext;
    private readonly DbSet<Poule> _poules;

    public PouleRepository(DesignDbContext designDbContext)
    {
        _designDbContext = designDbContext;
        _poules = designDbContext.Set<Poule>();
    }

    public async Task AddAsync(Poule poule)
    {
        ArgumentNullException.ThrowIfNull(poule, nameof(poule));

        _poules.Add(poule);
        await _designDbContext.SaveChangesAsync();
    }

    public async Task<ICollection<Poule>> GetAllByRoundAndTournamentAsync(TournamentId tournamentId, RoundId roundId)
    {
        ArgumentNullException.ThrowIfNull(tournamentId, nameof(tournamentId));
        ArgumentNullException.ThrowIfNull(roundId, nameof(roundId));

        return await _poules
            .Include(poule => poule.Round)
                .ThenInclude(round => round.Tournament)
            .Where(p => p.Round.Tournament.Id == tournamentId && p.Round.Id == roundId)
            .ToListAsync();
    }

    public async Task<Poule?> GetByIdAsync(PouleId id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));

        return await _poules.SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task RemoveAsync(Poule poule)
    {
        ArgumentNullException.ThrowIfNull(poule, nameof(poule));

        _poules.Remove(poule);
        await _designDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Poule poule)
    {
        ArgumentNullException.ThrowIfNull(poule, nameof(poule));

        _poules.Update(poule);
        await _designDbContext.SaveChangesAsync();
    }
}
