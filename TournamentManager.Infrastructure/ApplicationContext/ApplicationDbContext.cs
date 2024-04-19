using Microsoft.EntityFrameworkCore;
using TournamentManager.Domain;

namespace TournamentManager.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Round> Rounds { get; set; }
    public DbSet<Poule> Poules { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<TableTennisSettings> TableTennisSettings { get; set; }
    public DbSet<TableTennisRoundSettings> TableTennisRoundSettings { get; set; }
    
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Tournament>()
            .HasOne(u => u.Settings)
            .WithOne(u => u.Tournament)
            .HasForeignKey<TournamentSettings>(u => u.TournamentId);
    }
}
