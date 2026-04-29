using Competition.Domain.Interfaces;
using Competition.Domain.ValueObjects;
using Competition.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using CompetitionEntity = Competition.Domain.Entities.Competition;

namespace Competition.Infrastructure.Repositories;

public class CompetitionRepository : ICompetitionRepository
{
    private readonly CompetitionDbContext _competitionDbContext;
    private readonly DbSet<CompetitionEntity> _competitions;

    public CompetitionRepository(CompetitionDbContext competitionDbContext)
    {
        _competitionDbContext = competitionDbContext;
        _competitions = competitionDbContext.Set<CompetitionEntity>();
    }

    public async Task AddAsync(CompetitionEntity competition)
    {
        ArgumentNullException.ThrowIfNull(competition, nameof(competition));

        await _competitions.AddAsync(competition);
        await _competitionDbContext.SaveChangesAsync();
    }

    public async Task<CompetitionEntity?> GetByIdAsync(CompetitionId id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));

        return await _competitions.FindAsync(id);
    }

    public async Task<ICollection<CompetitionEntity>> GetAllAsync()
    {
        return await _competitions.ToListAsync();
    }
}
