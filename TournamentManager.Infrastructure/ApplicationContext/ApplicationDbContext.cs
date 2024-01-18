using Microsoft.EntityFrameworkCore;

namespace TournamentManager.Infrastructure;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
}
