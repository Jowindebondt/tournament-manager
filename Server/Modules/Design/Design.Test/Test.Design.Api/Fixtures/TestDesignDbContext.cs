using Design.Domain.Entities;
using Design.Domain.ValueObjects;
using Design.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Test.Design.Api.Fixtures;

/// <summary>
/// A test-only DbContext that extends DesignDbContext with the EF Core model configuration
/// required for the InMemory provider. The production DesignDbContext relies on custom
/// EF Core configurations managed outside the DbContext class itself; this derived context
/// supplies the necessary value converters and relationship mappings for in-memory tests.
/// </summary>
public class TestDesignDbContext : DesignDbContext
{
    public TestDesignDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id)
                .HasConversion(id => id.Value, value => new TournamentId(value));
            entity.Property(t => t.Name)
                .HasConversion(n => n.Value, value => TournamentName.Create(value));
        });

        modelBuilder.Entity<Round>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id)
                .HasConversion(id => id.Value, value => new RoundId(value));
            entity.Property(r => r.Name)
                .HasConversion(n => n.Value, value => RoundName.Create(value));
            entity.Property(r => r.TournamentId)
                .HasConversion(id => id.Value, value => new TournamentId(value));
            entity.HasOne(r => r.Tournament)
                .WithMany(t => t.Rounds)
                .HasForeignKey(r => r.TournamentId);
            entity.Ignore(r => r.Settings);
            entity.Ignore(r => r.Type);
            entity.Ignore(r => r.PreviousRound);
            entity.Ignore(r => r.NextRound);
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
    }
}
