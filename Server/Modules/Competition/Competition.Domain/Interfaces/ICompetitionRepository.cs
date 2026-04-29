using Competition.Domain.ValueObjects;
using CompetitionEntity = Competition.Domain.Entities.Competition;

namespace Competition.Domain.Interfaces;

public interface ICompetitionRepository
{
    Task AddAsync(CompetitionEntity competition);
    Task<CompetitionEntity?> GetByIdAsync(CompetitionId id);
    Task<ICollection<CompetitionEntity>> GetAllAsync();
}
