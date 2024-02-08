using Microsoft.EntityFrameworkCore;
using TournamentManager.Domain.Test;
using TournamentManager.Infrastructure;

namespace TournamentManager.Api.Test;

/// <summary>
/// Class defining the <see cref="DatabaseFixture"/> containing an in memory database for test purposes.
/// </summary>
public abstract class DatabaseFixture : IDisposable
{
    /// <summary>
    /// Reference to the <see cref="ApplicationDbContext"/> which contains an in memory database.
    /// </summary>
    public ApplicationDbContext DbContext { get; }

    /// <summary>
    /// Initializing a new instance of <see cref="DatabaseFixture"/>.
    /// Creates the base setup of the in memory database.
    /// </summary>
    protected DatabaseFixture(string databaseName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: databaseName)
            .Options;

        DbContext = new ApplicationDbContext(options);

        FillContext();
        DbContext.SaveChanges();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        CleanContext();
        DbContext.SaveChanges();
    }

    protected abstract void CleanContext();
    protected abstract void FillContext();
}
