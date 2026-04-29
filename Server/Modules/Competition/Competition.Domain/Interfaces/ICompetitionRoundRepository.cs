using Competition.Domain.Entities;
using Competition.Domain.ValueObjects;

namespace Competition.Domain.Interfaces;

public interface ICompetitionRoundRepository
{
    Task<ICollection<CompetitionRound>> GetAllByCompetitionAsync(CompetitionId competitionId);
    Task<CompetitionRound?> GetByIdAsync(CompetitionRoundId id);
    Task AddAsync(CompetitionRound round);
    Task UpdateAsync(CompetitionRound round);
    Task RemoveAsync(CompetitionRound round);
}
