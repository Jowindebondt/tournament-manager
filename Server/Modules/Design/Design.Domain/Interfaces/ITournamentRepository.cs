using Design.Domain.Entities;
using Design.Domain.ValueObjects;

namespace Design.Domain.Interfaces;

public interface ITournamentRepository
{
    Task<ICollection<Tournament>> GetAllAsync();
    Task<Tournament?> GetByIdAsync(TournamentId id);
    Task AddAsync(Tournament tournament);
    Task UpdateAsync(Tournament tournament);
    Task RemoveAsync(Tournament tournament);
}
