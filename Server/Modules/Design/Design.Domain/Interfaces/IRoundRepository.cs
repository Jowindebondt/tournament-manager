using Design.Domain.Entities;
using Design.Domain.ValueObjects;

namespace Design.Domain.Interfaces;

public interface IRoundRepository
{
    Task<ICollection<Round>> GetAllByTournamentAsync(TournamentId tournamentId);
    Task<Round?> GetByIdAsync(RoundId id);
    Task AddAsync(Round round);
    Task UpdateAsync(Round round);
    Task RemoveAsync(Round round);
}
