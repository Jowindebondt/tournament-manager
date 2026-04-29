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
