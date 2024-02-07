using Microsoft.EntityFrameworkCore;
using TournamentManager.Domain.Test;
using TournamentManager.Infrastructure;

namespace TournamentManager.Api.Test;

public class DatabaseFixture : IDisposable
{
    public ApplicationDbContext DbContext { get; }

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TournamentManagerDB")
            .Options;

        DbContext = new ApplicationDbContext(options);

        for (var i = 0; i < 10; i++)
        {
            DbContext.Tournaments.Add(TournamentBuilder.GetSingleTournament(i + 1));
        }
        DbContext.SaveChanges();
    }

    public void Dispose()
    {
        DbContext.Tournaments.RemoveRange(DbContext.Tournaments);
        DbContext.SaveChanges();
    }
}
