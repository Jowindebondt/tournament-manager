using CompetitionEntity = Competition.Domain.Entities.Competition;
using Competition.Domain.Entities;
using Competition.Domain.ValueObjects;
using Competition.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Test.Design.Api.Fixtures;

/// <summary>
/// A test-only DbContext that extends CompetitionDbContext with the EF Core model configuration
/// required for the InMemory provider.
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

        modelBuilder.Entity<CompetitionRound>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id)
                .HasConversion(id => id.Value, value => new CompetitionRoundId(value));
            entity.Property(r => r.Name)
                .HasConversion(n => n.Value, value => CompetitionRoundName.Create(value));
            entity.Property(r => r.CompetitionId)
                .HasConversion(id => id.Value, value => new CompetitionId(value));
            entity.HasOne(r => r.Competition)
                .WithMany(c => c.Rounds)
                .HasForeignKey(r => r.CompetitionId);
            entity.Ignore(r => r.Plan);
        });

        modelBuilder.Entity<CompetitionPoule>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id)
                .HasConversion(id => id.Value, value => new CompetitionPouleId(value));
            entity.Property(p => p.Name)
                .HasConversion(n => n.Value, value => CompetitionPouleName.Create(value));
            entity.Property(p => p.CompetitionRoundId)
                .HasConversion(id => id.Value, value => new CompetitionRoundId(value));
            entity.HasOne(p => p.CompetitionRound)
                .WithMany(r => r.Poules)
                .HasForeignKey(p => p.CompetitionRoundId);
        });
    }
}
