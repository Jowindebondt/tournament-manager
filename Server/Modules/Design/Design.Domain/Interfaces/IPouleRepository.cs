using Design.Domain.Entities;
using Design.Domain.ValueObjects;

namespace Design.Domain.Interfaces;

public interface IPouleRepository
{
    Task<ICollection<Poule>> GetAllByRoundAndTournamentAsync(TournamentId tournamentId, RoundId roundId);
    Task<Poule?> GetByIdAsync(PouleId id);
    Task AddAsync(Poule poule);
    Task UpdateAsync(Poule poule);
    Task RemoveAsync(Poule poule);
}
