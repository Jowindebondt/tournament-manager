using Design.Domain.Entities;
using Design.Domain.ValueObjects;
using Design.Infrastructure.Persistence;
using Design.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Test.Design.Infrastructure;

public class UT_PouleRepository
{
    private readonly Mock<DbSet<Poule>> _dbSet;
    private readonly Mock<DesignDbContext> _dbContextMock;
    private PouleRepository _repo;

    public UT_PouleRepository()
    {
        var dbContextOptions = new DbContextOptionsBuilder<DesignDbContext>().Options;

        _dbSet = new Mock<DbSet<Poule>>();
        _dbContextMock = new Mock<DesignDbContext>(dbContextOptions);
        _dbContextMock.Setup(context => context.Set<Poule>()).Returns(_dbSet.Object);
        _repo = new PouleRepository(_dbContextMock.Object);
    }

    [Fact]
    public async Task AddAsync_ValidValue_AddsPouleAndSavesChanges()
    {
        // arrange
        Poule addedPoule = null!;

        var pouleId = new PouleId(Guid.NewGuid());
        var pouleName = PouleName.Create("abc");
        var totalPlayers = PoulePlayersCount.Create(4);
        var roundId = new RoundId(Guid.NewGuid());
        var poule = new Poule(pouleId, pouleName, totalPlayers, roundId);

        _dbSet.Setup(set => set.Add(It.IsAny<Poule>())).Callback((Poule addPoule) => addedPoule = addPoule);

        // act
        await _repo.AddAsync(poule);

        // assert
        Assert.Multiple(
            () => Assert.Equal(poule, addedPoule),
            () => _dbSet.Verify(set => set.Add(It.IsAny<Poule>()), Times.Once),
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
            () => _dbSet.Verify(set => set.Add(It.IsAny<Poule>()), Times.Never),
            () => _dbContextMock.Verify(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never)
        );
    }

    [Fact]
    public async Task GetAllByTournamentAndRoundAsync_NullTournamentId_ThrowsArgumentNullException()
    {
        // arrange
        var roundId = new RoundId(Guid.NewGuid());

        // act & assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _repo.GetAllByTournamentAndRoundAsync(null!, roundId));
    }

    [Fact]
    public async Task GetAllByTournamentAndRoundAsync_NullRoundId_ThrowsArgumentNullException()
    {
        // arrange
        var tournamentId = new TournamentId(Guid.NewGuid());

        // act & assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _repo.GetAllByTournamentAndRoundAsync(tournamentId, null!));
    }

    [Fact]
    public async Task GetByIdAsync_NullValue_ThrowsArgumentNullException()
    {
        // arrange

        // act & assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _repo.GetByIdAsync(null!));
    }

    [Fact]
    public async Task RemoveAsync_ValidValue_RemovesPouleAndSavesChanges()
    {
        // arrange
        Poule removedPoule = null!;

        var pouleId = new PouleId(Guid.NewGuid());
        var pouleName = PouleName.Create("abc");
        var totalPlayers = PoulePlayersCount.Create(4);
        var roundId = new RoundId(Guid.NewGuid());
        var poule = new Poule(pouleId, pouleName, totalPlayers, roundId);

        _dbSet.Setup(set => set.Remove(It.IsAny<Poule>())).Callback((Poule removePoule) => removedPoule = removePoule);

        // act
        await _repo.RemoveAsync(poule);

        // assert
        Assert.Multiple(
            () => Assert.Equal(poule, removedPoule),
            () => _dbSet.Verify(set => set.Remove(It.IsAny<Poule>()), Times.Once),
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
            () => _dbSet.Verify(set => set.Remove(It.IsAny<Poule>()), Times.Never),
            () => _dbContextMock.Verify(set => set.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never)
        );
    }

    [Fact]
    public async Task UpdateAsync_ValidValue_UpdatesPouleAndSavesChanges()
    {
        // arrange
        Poule updatedPoule = null!;

        var pouleId = new PouleId(Guid.NewGuid());
        var pouleName = PouleName.Create("abc");
        var totalPlayers = PoulePlayersCount.Create(4);
        var roundId = new RoundId(Guid.NewGuid());
        var poule = new Poule(pouleId, pouleName, totalPlayers, roundId);

        _dbSet.Setup(set => set.Update(It.IsAny<Poule>())).Callback((Poule updatePoule) => updatedPoule = updatePoule);

        // act
        await _repo.UpdateAsync(poule);

        // assert
        Assert.Multiple(
            () => Assert.Equal(poule, updatedPoule),
            () => _dbSet.Verify(set => set.Update(It.IsAny<Poule>()), Times.Once),
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
            () => _dbSet.Verify(set => set.Update(It.IsAny<Poule>()), Times.Never),
            () => _dbContextMock.Verify(set => set.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never)
        );
    }
}
