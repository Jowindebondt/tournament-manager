using Microsoft.EntityFrameworkCore;
using CompetitionEntity = Competition.Domain.Entities.Competition;
using Round = Competition.Domain.Entities.Round;
using Poule = Competition.Domain.Entities.Poule;

namespace Competition.Infrastructure.Persistence;

public class CompetitionDbContext : DbContext
{
    public DbSet<CompetitionEntity> Competitions { get; set; } = null!;
    public DbSet<Round> Rounds { get; set; } = null!;
    public DbSet<Poule> Poules { get; set; } = null!;

    public CompetitionDbContext(DbContextOptions options) : base(options)
    {
    }
}
