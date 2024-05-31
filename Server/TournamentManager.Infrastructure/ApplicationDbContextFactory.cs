using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TournamentManager.Infrastructure;

/// <summary>
/// Factory will be used for EF actions during design-time
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        // optionsBuilder.UseSqlServer("Server=tcp:jpl.database.windows.net,1433;Initial Catalog=jpl;Persist Security Info=False;User ID=hihqeatnrn;Password=2KVL15UOI26WY461$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=TournamentManager;User ID=SA;Password=Welcome01!;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
