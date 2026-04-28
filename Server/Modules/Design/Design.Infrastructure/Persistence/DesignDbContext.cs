using Design.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Design.Infrastructure.Persistence;

public class DesignDbContext : DbContext
{
    public DbSet<Tournament> Tournaments { get; set; } = null!;
    public DbSet<Round> Rounds { get; set; } = null!;
    public DbSet<Poule> Poules { get; set; } = null!;

    public DesignDbContext(DbContextOptions options) : base(options)
    {
    }
}
