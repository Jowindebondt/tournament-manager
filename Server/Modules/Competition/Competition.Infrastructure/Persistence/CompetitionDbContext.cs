using Competition.Domain.Entities;
using Competition.Domain.Enums;
using Competition.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Competition.Infrastructure.Persistence;

public class CompetitionDbContext : DbContext
{
    private const string KnockOutPrefix = "KnockOut:";

    public DbSet<Competition.Domain.Entities.Competition> Competitions { get; set; } = null!;
    public DbSet<CompetitionRound> CompetitionRounds { get; set; } = null!;
    public DbSet<CompetitionPoule> CompetitionPoules { get; set; } = null!;
    public DbSet<Competitor> Competitors { get; set; } = null!;
    public DbSet<RoundRobinGame> RoundRobinGames { get; set; } = null!;
    public DbSet<BracketGame> BracketGames { get; set; } = null!;

    public CompetitionDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Competition.Domain.Entities.Competition>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id)
                .HasConversion(id => id.Value, value => new CompetitionId(value));
            entity.Property(c => c.Name)
                .HasConversion(n => n.Value, value => CompetitionName.Create(value));
            entity.Property(c => c.Sport)
                .HasConversion<string>();
            entity.Property(c => c.Status)
                .HasConversion<string>();
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
            entity.Property(r => r.Plan)
                .HasColumnName("Plan")
                .HasConversion(
                    plan => plan == null ? null : SerializePlan(plan),
                    str => str == null ? null : DeserializePlan(str));
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

        modelBuilder.Entity<Competitor>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id)
                .HasConversion(id => id.Value, value => new CompetitorId(value));
            entity.Property(c => c.Name)
                .HasConversion(n => n.Value, value => CompetitorName.Create(value));
            entity.Property(c => c.CompetitionPouleId)
                .HasConversion(id => id.Value, value => new CompetitionPouleId(value));
            entity.HasOne(c => c.CompetitionPoule)
                .WithMany(p => p.Competitors)
                .HasForeignKey(c => c.CompetitionPouleId);
        });

        modelBuilder.Entity<RoundRobinGame>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Id)
                .HasConversion(id => id.Value, value => new RoundRobinGameId(value));
            entity.Property(g => g.CompetitionPouleId)
                .HasConversion(id => id.Value, value => new CompetitionPouleId(value));
            entity.HasOne(g => g.CompetitionPoule)
                .WithMany(p => p.Games)
                .HasForeignKey(g => g.CompetitionPouleId);
            entity.Property(g => g.HomeCompetitorId)
                .HasConversion(id => id.Value, value => new CompetitorId(value));
            entity.HasOne(g => g.HomeCompetitor)
                .WithMany()
                .HasForeignKey(g => g.HomeCompetitorId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.Property(g => g.AwayCompetitorId)
                .HasConversion(id => id.Value, value => new CompetitorId(value));
            entity.HasOne(g => g.AwayCompetitor)
                .WithMany()
                .HasForeignKey(g => g.AwayCompetitorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<BracketGame>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Id)
                .HasConversion(id => id.Value, value => new BracketGameId(value));
            entity.Property(g => g.CompetitionRoundId)
                .HasConversion(id => id.Value, value => new CompetitionRoundId(value));
            entity.HasOne(g => g.CompetitionRound)
                .WithMany(r => r.BracketGames)
                .HasForeignKey(g => g.CompetitionRoundId);
            entity.Property(g => g.HomeCompetitorId)
                .HasConversion(id => id!.Value, value => new CompetitorId(value));
            entity.HasOne(g => g.HomeCompetitor)
                .WithMany()
                .HasForeignKey(g => g.HomeCompetitorId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.Property(g => g.AwayCompetitorId)
                .HasConversion(id => id!.Value, value => new CompetitorId(value));
            entity.HasOne(g => g.AwayCompetitor)
                .WithMany()
                .HasForeignKey(g => g.AwayCompetitorId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    internal static string SerializePlan(CompetitionPlan plan) => plan switch
    {
        RoundRobinPlan => "RoundRobin",
        KnockOutPlan ko => $"{KnockOutPrefix}{ko.Phase}",
        _ => throw new ArgumentOutOfRangeException(nameof(plan), $"Unsupported plan type: {plan.GetType().Name}")
    };

    internal static CompetitionPlan DeserializePlan(string value)
    {
        if (value == "RoundRobin")
            return new RoundRobinPlan();

        if (value.StartsWith(KnockOutPrefix) && Enum.TryParse<KnockOutPhase>(value[KnockOutPrefix.Length..], out var phase))
            return new KnockOutPlan(phase);

        throw new ArgumentException($"Cannot deserialize CompetitionPlan from '{value}'.");
    }
}
