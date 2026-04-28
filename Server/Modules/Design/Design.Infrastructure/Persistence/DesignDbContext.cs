using Design.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Design.Infrastructure.Persistence;

public class DesignDbContext : DbContext
{
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Round> Rounds { get; set; }
    public DbSet<Poule> Poules { get; set; }

    public DesignDbContext(DbContextOptions options) : base(options)
    {
    }
}
