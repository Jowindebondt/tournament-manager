using Design.Domain.Entities;
using Design.Domain.ValueObjects;

namespace Design.Domain;

public interface IPouleRepository
{
    Task<IEnumerable<Poule>> GetAllByTournamentAndRoundAsync(TournamentId tournamentId, RoundId roundId);
    Task<Poule> GetByIdAsync(PouleId id);
    Task AddAsync(Poule poule);
    Task UpdateAsync(Poule poule);
    Task RemoveAsync(Poule poule);
}
