using Competition.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Competition.Infrastructure.Persistence;

public class CompetitionDbContext : DbContext
{
    public DbSet<Competition.Domain.Entities.Competition> Competitions { get; set; } = null!;
    public DbSet<CompetitionRound> CompetitionRounds { get; set; } = null!;
    public DbSet<CompetitionPoule> CompetitionPoules { get; set; } = null!;

    public CompetitionDbContext(DbContextOptions options) : base(options)
    {
    }
}
