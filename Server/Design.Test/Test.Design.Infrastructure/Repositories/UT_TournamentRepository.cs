using Design.Domain.Entities;
using Design.Domain.Enums;
using Design.Domain.ValueObjects;
using Design.Infrastructure.Persistence;
using Design.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Test.Design.Infrastructure;

public class UT_TournamentRepository
{
    private readonly Mock<DbSet<Tournament>> _dbSet;
    private readonly Mock<DesignDbContext> _dbContextMock;
    private TournamentRepository _repo;

    public UT_TournamentRepository()
    {
        var dbContextOptions = new DbContextOptionsBuilder<DesignDbContext>().Options;

        _dbSet = new Mock<DbSet<Tournament>>();
        _dbContextMock = new Mock<DesignDbContext>(dbContextOptions);
        _dbContextMock.Setup(context => context.Set<Tournament>()).Returns(_dbSet.Object);
        _repo = new TournamentRepository(_dbContextMock.Object);
    }

    [Fact]
    public async Task AddAsync_ValidValue_AddsTournamentAndSavesChanges()
    {
        // arrange
        Tournament addedTournament = null!;
        
        var tournamentId = new TournamentId(Guid.NewGuid());
        var tournamentName = TournamentName.Create("abc");
        var tournament = new Tournament(tournamentId, tournamentName, Sport.TableTennis);

        _dbSet.Setup(set => set.AddAsync(It.IsAny<Tournament>(), It.IsAny<CancellationToken>())).Callback((Tournament addTournament, CancellationToken _) => addedTournament = addTournament);

        // act
        await _repo.AddAsync(tournament);

        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.AddAsync(It.IsAny<Tournament>(), It.IsAny<CancellationToken>()), Times.Once),
            () => _dbContextMock.Verify(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once)
        );
    }

    [Fact]
    public async Task AddAsync_NullValue_ThrowsArgumentNullException()
    {
        // arrange

        // act & assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _repo.AddAsync(null!));
        
        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.AddAsync(It.IsAny<Tournament>(), It.IsAny<CancellationToken>()), Times.Never),
            () => _dbContextMock.Verify(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never)
        );
    }

    [Fact]
    public async Task GetAllAsync_ReturnsTournaments()
    {
        // arrange
        var tournaments = new List<Tournament>();
        for (var i = 0; i < 10; i++)
        {
            var tournamentId = new TournamentId(Guid.NewGuid());
            var tournamentName = TournamentName.Create($"TestTournament_{i}");
            var tournament = new Tournament(tournamentId, tournamentName, Sport.TableTennis);
            tournaments.Add(tournament);
        }

        _dbSet.Setup(set => set.ToListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(tournaments);

        // act
        var result = await _repo.GetAllAsync();

        // assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.NotEmpty(result),
            () => Assert.Equal(10, result.Count),
            () => Assert.Equal(tournaments, result),
            () => _dbSet.Verify(set => set.ToListAsync(It.IsAny<CancellationToken>()), Times.Once)
        );
    }

    [Fact]
    public async Task GetByIdAsync_ValidValue_ReturnsValue()
    {
        // arrange
        var guid = Guid.NewGuid();
        var tournamentId = new TournamentId(guid);
        var tournamentName = TournamentName.Create($"abc");
        var tournament = new Tournament(tournamentId, tournamentName, Sport.TableTennis);

        _dbSet.Setup(set => set.FindAsync(It.IsAny<TournamentId>())).ReturnsAsync(tournament);

        // act
        var result = await _repo.GetByIdAsync(new TournamentId(guid));

        // assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(tournament, result),
            () => _dbSet.Verify(set => set.SingleAsync(It.IsAny<CancellationToken>()), Times.Once)
        );
    }

    [Fact]
    public async Task GetByIdAsync_NullValue_ThrowsArgumentNullException()
    {
        // arrange

        // act
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _repo.GetByIdAsync(null!));

        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.SingleAsync(It.IsAny<CancellationToken>()), Times.Never)
        );
    }

    [Fact]
    public async Task RemoveAsync_ValidValue_RemovesTournamentAndSavesChanges()
    {
        // arrange
        Tournament removedTournament = null!;

        var tournamentId = new TournamentId(Guid.NewGuid());
        var tournamentName = TournamentName.Create($"abc");
        var tournament = new Tournament(tournamentId, tournamentName, Sport.TableTennis);

        _dbSet.Setup(set => set.Remove(It.IsAny<Tournament>())).Callback((Tournament removeTournament) => removedTournament = removeTournament);

        // act
        await _repo.RemoveAsync(tournament);

        // assert
        Assert.Multiple(
            () => Assert.Equal(tournament, removedTournament),
            () => _dbSet.Verify(set => set.Remove(It.IsAny<Tournament>()), Times.Once),
            () => _dbContextMock.Verify(set => set.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once)
        );
    }

    [Fact]
    public async Task RemoveAsync_NullValue_ThrowsArgumentNullException()
    {
        // arrange

        // act & assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _repo.RemoveAsync(null!));

        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.Remove(It.IsAny<Tournament>()), Times.Never),
            () => _dbContextMock.Verify(set => set.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never)
        );
    }

    [Fact]
    public async Task UpdateAsync_ValidValue_UpdatesTournamentAndSavesChanges()
    {
        // arrange
        Tournament updatedTournament = null!;

        var tournamentId = new TournamentId(Guid.NewGuid());
        var tournamentName = TournamentName.Create($"abc");
        var tournament = new Tournament(tournamentId, tournamentName, Sport.TableTennis);

        _dbSet.Setup(set => set.Update(It.IsAny<Tournament>())).Callback((Tournament updateTournament) => updatedTournament = updateTournament);

        // act
        await _repo.UpdateAsync(tournament);

        // assert
        Assert.Multiple(
            () => Assert.Equal(tournament, updatedTournament),
            () => _dbSet.Verify(set => set.Update(It.IsAny<Tournament>()), Times.Once),
            () => _dbContextMock.Verify(set => set.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once)
        );
    }

    [Fact]
    public async Task UpdateAsync_NullValue_ThrowsArgumentNullException()
    {
        // arrange

        // act & assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _repo.UpdateAsync(null!));

        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.Update(It.IsAny<Tournament>()), Times.Never),
            () => _dbContextMock.Verify(set => set.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never)
        );
    }
}
