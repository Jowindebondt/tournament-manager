using Microsoft.EntityFrameworkCore;
using TournamentManager.Domain.Test;
using TournamentManager.Infrastructure;

namespace TournamentManager.Api.Test;

/// <summary>
/// Class defining the <see cref="DatabaseFixture"/> containing an in memory database for test purposes.
/// </summary>
public class DatabaseFixture : IDisposable
{
    /// <summary>
    /// Reference to the <see cref="ApplicationDbContext"/> which contains an in memory database.
    /// </summary>
    public ApplicationDbContext DbContext { get; }

    /// <summary>
    /// Initializing a new instance of <see cref="DatabaseFixture"/>.
    /// Creates the base setup of the in memory database.
    /// </summary>
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

    /// <inheritdoc/>
    public void Dispose()
    {
        DbContext.Tournaments.RemoveRange(DbContext.Tournaments);
        DbContext.SaveChanges();
    }
}
