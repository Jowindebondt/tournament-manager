using Competition.Domain.Entities;
using Competition.Domain.ValueObjects;

namespace Competition.Domain.Interfaces;

public interface ICompetitionPouleRepository
{
    Task<ICollection<CompetitionPoule>> GetAllByRoundAndCompetitionAsync(CompetitionId competitionId, CompetitionRoundId roundId);
    Task<CompetitionPoule?> GetByIdAsync(CompetitionPouleId id);
    Task AddAsync(CompetitionPoule poule);
    Task UpdateAsync(CompetitionPoule poule);
    Task RemoveAsync(CompetitionPoule poule);
}
