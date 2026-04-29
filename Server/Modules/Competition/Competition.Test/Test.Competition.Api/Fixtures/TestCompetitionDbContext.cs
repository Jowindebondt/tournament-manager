using Competition.Domain.ValueObjects;
using Competition.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using CompetitionEntity = Competition.Domain.Entities.Competition;
using Round = Competition.Domain.Entities.Round;
using Poule = Competition.Domain.Entities.Poule;
using Match = Competition.Domain.Entities.Match;

namespace Test.Competition.Api.Fixtures;

/// <summary>
/// A test-only DbContext that extends CompetitionDbContext with the EF Core model configuration
/// required for the InMemory provider. Supplies the necessary value converters and relationship
/// mappings for in-memory tests.
/// </summary>
public class TestCompetitionDbContext : CompetitionDbContext
{
    public TestCompetitionDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CompetitionEntity>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id)
                .HasConversion(id => id.Value, value => new CompetitionId(value));
            entity.Property(c => c.Name)
                .HasConversion(n => n.Value, value => CompetitionName.Create(value));
        });

        modelBuilder.Entity<Round>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id)
                .HasConversion(id => id.Value, value => new RoundId(value));
            entity.Property(r => r.Name)
                .HasConversion(n => n.Value, value => RoundName.Create(value));
            entity.Property(r => r.CompetitionId)
                .HasConversion(id => id.Value, value => new CompetitionId(value));
            entity.HasOne(r => r.Competition)
                .WithMany(c => c.Rounds)
                .HasForeignKey(r => r.CompetitionId);
            entity.Ignore(r => r.Plan);
        });

        modelBuilder.Entity<Poule>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id)
                .HasConversion(id => id.Value, value => new PouleId(value));
            entity.Property(p => p.Name)
                .HasConversion(n => n.Value, value => PouleName.Create(value));
            entity.Property(p => p.TotalPlayers)
                .HasConversion(c => c.Value, value => PoulePlayersCount.Create(value));
            entity.Property(p => p.RoundId)
                .HasConversion(id => id.Value, value => new RoundId(value));
            entity.HasOne(p => p.Round)
                .WithMany(r => r.Poules)
                .HasForeignKey(p => p.RoundId);
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Id)
                .HasConversion(id => id.Value, value => new MatchId(value));
            entity.Property(m => m.PouleId)
                .HasConversion(id => id.Value, value => new PouleId(value));
            entity.HasOne(m => m.Poule)
                .WithMany(p => p.Matches)
                .HasForeignKey(m => m.PouleId);
            entity.Property(m => m.Result)
                .HasConversion(
                    result => result == null ? null : $"{result.Player1Score}:{result.Player2Score}",
                    value => value == null ? null : new MatchResult(
                        short.Parse(value.Split(':')[0]),
                        short.Parse(value.Split(':')[1])));
        });
    }
}
