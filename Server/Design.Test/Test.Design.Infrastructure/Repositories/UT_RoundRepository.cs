using Design.Domain.Entities;
using Design.Domain.ValueObjects;
using Design.Infrastructure.Persistence;
using Design.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Test.Design.Infrastructure;

public class UT_RoundRepository
{
    private readonly Mock<DbSet<Round>> _dbSet;
    private readonly Mock<DesignDbContext> _dbContextMock;
    private RoundRepository _repo;

    public UT_RoundRepository()
    {
        var dbContextOptions = new DbContextOptionsBuilder<DesignDbContext>().Options;

        _dbSet = new Mock<DbSet<Round>>();
        _dbContextMock = new Mock<DesignDbContext>(dbContextOptions);
        _dbContextMock.Setup(context => context.Set<Round>()).Returns(_dbSet.Object);
        _repo = new RoundRepository(_dbContextMock.Object);
    }

    [Fact]
    public async Task AddAsync_ValidValue_AddsRoundAndSavesChanges()
    {
        // arrange
        Round addedRound = null!;

        var roundId = new RoundId(Guid.NewGuid());
        var roundName = RoundName.Create("abc");
        var tournamentId = new TournamentId(Guid.NewGuid());
        var round = new Round(roundId, roundName, tournamentId);

        _dbSet.Setup(set => set.Add(It.IsAny<Round>())).Callback((Round addRound) => addedRound = addRound);

        // act
        await _repo.AddAsync(round);

        // assert
        Assert.Multiple(
            () => Assert.Equal(round, addedRound),
            () => _dbSet.Verify(set => set.Add(It.IsAny<Round>()), Times.Once),
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
            () => _dbSet.Verify(set => set.Add(It.IsAny<Round>()), Times.Never),
            () => _dbContextMock.Verify(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never)
        );
    }

    [Fact]
    public async Task GetAllByTournamentAsync_ValidValue_ReturnsRounds()
    {
        // arrange
        var tournamentId = new TournamentId(Guid.NewGuid());
        var rounds = new List<Round>();
        for (var i = 0; i < 10; i++)
        {
            var roundId = new RoundId(Guid.NewGuid());
            var roundName = RoundName.Create($"TestRound_{i}");
            var round = new Round(roundId, roundName, tournamentId);
            rounds.Add(round);
        }

        _dbSet.Setup(set => set.ToListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(rounds);

        // act
        var result = await _repo.GetAllByTournamentAsync(tournamentId);

        // assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.NotEmpty(result),
            () => Assert.Equal(10, result.Count),
            () => Assert.Equal(rounds, result),
            () => _dbSet.Verify(set => set.ToListAsync(It.IsAny<CancellationToken>()), Times.Once)
        );
    }

    [Fact]
    public async Task GetAllByTournamentAsync_NullValue_ThrowsArgumentNullException()
    {
        // arrange

        // act
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _repo.GetAllByTournamentAsync(null!));

        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.ToListAsync(It.IsAny<CancellationToken>()), Times.Never)
        );
    }

    [Fact]
    public async Task GetByIdAsync_ValidValue_ReturnsValue()
    {
        // arrange
        var guid = Guid.NewGuid();
        var roundId = new RoundId(guid);
        var roundName = RoundName.Create("abc");
        var tournamentId = new TournamentId(Guid.NewGuid());
        var round = new Round(roundId, roundName, tournamentId);

        _dbSet.Setup(set => set.SingleAsync(It.IsAny<CancellationToken>())).ReturnsAsync(round);

        // act
        var result = await _repo.GetByIdAsync(new RoundId(guid));

        // assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(round, result),
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
    public async Task RemoveAsync_ValidValue_RemovesRoundAndSavesChanges()
    {
        // arrange
        Round removedRound = null!;

        var roundId = new RoundId(Guid.NewGuid());
        var roundName = RoundName.Create("abc");
        var tournamentId = new TournamentId(Guid.NewGuid());
        var round = new Round(roundId, roundName, tournamentId);

        _dbSet.Setup(set => set.Remove(It.IsAny<Round>())).Callback((Round removeRound) => removedRound = removeRound);

        // act
        await _repo.RemoveAsync(round);

        // assert
        Assert.Multiple(
            () => Assert.Equal(round, removedRound),
            () => _dbSet.Verify(set => set.Remove(It.IsAny<Round>()), Times.Once),
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
            () => _dbSet.Verify(set => set.Remove(It.IsAny<Round>()), Times.Never),
            () => _dbContextMock.Verify(set => set.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never)
        );
    }

    [Fact]
    public async Task UpdateAsync_ValidValue_UpdatesRoundAndSavesChanges()
    {
        // arrange
        Round updatedRound = null!;

        var roundId = new RoundId(Guid.NewGuid());
        var roundName = RoundName.Create("abc");
        var tournamentId = new TournamentId(Guid.NewGuid());
        var round = new Round(roundId, roundName, tournamentId);

        _dbSet.Setup(set => set.Update(It.IsAny<Round>())).Callback((Round updateRound) => updatedRound = updateRound);

        // act
        await _repo.UpdateAsync(round);

        // assert
        Assert.Multiple(
            () => Assert.Equal(round, updatedRound),
            () => _dbSet.Verify(set => set.Update(It.IsAny<Round>()), Times.Once),
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
            () => _dbSet.Verify(set => set.Update(It.IsAny<Round>()), Times.Never),
            () => _dbContextMock.Verify(set => set.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never)
        );
    }
}
