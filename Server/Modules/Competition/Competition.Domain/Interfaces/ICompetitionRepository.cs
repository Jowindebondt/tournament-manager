using Competition.Domain.ValueObjects;

namespace Competition.Domain.Interfaces;

public interface ICompetitionRepository
{
    Task<ICollection<Entities.Competition>> GetAllAsync();
    Task<Entities.Competition?> GetByIdAsync(CompetitionId id);
    Task AddAsync(Entities.Competition competition);
    Task UpdateAsync(Entities.Competition competition);
    Task RemoveAsync(Entities.Competition competition);
}
