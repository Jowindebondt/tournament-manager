using Microsoft.EntityFrameworkCore;
using CompetitionEntity = Competition.Domain.Entities.Competition;
using Round = Competition.Domain.Entities.Round;
using Poule = Competition.Domain.Entities.Poule;
using Match = Competition.Domain.Entities.Match;

namespace Competition.Infrastructure.Persistence;

public class CompetitionDbContext : DbContext
{
    public DbSet<CompetitionEntity> Competitions { get; set; } = null!;
    public DbSet<Round> Rounds { get; set; } = null!;
    public DbSet<Poule> Poules { get; set; } = null!;
    public DbSet<Match> Matches { get; set; } = null!;

    public CompetitionDbContext(DbContextOptions options) : base(options)
    {
    }
}
