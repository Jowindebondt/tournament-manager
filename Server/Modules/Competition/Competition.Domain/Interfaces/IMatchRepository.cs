using Competition.Domain.Entities;
using Competition.Domain.ValueObjects;

namespace Competition.Domain.Interfaces;

public interface IMatchRepository
{
    Task<ICollection<Match>> GetAllByPouleAsync(PouleId pouleId);
    Task<Match?> GetByIdAsync(MatchId id);
    Task UpdateAsync(Match match);
}
