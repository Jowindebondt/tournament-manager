using Design.Domain.Entities;
using Design.Domain.ValueObjects;
using Xunit;

namespace Test.Design.Domain.Entities;

public class UT_Poule
{
    [Fact]
    public void Constructor_ValidValues_InitializesTournament()
    {
        // arrange
        var pouleId = new PouleId(Guid.NewGuid());
        var pouleName = PouleName.Create("abc");
        var poulePlayersCount = PoulePlayersCount.Create(4);
        var roundId = new RoundId(Guid.NewGuid());

        // act
        var value = new Poule(pouleId, pouleName, poulePlayersCount, roundId);

        // assert
        Assert.Multiple(
            () => Assert.NotNull(value),
            () => Assert.Equal(pouleId, value.Id),
            () => Assert.Equal(pouleName, value.Name),
            () => Assert.Equal(poulePlayersCount, value.TotalPlayers),
            () => Assert.Equal(roundId, value.RoundId)
        );
    }

    [Theory]
    [InlineData(false, true, true, true)]
    [InlineData(true, false, true, true)]
    [InlineData(true, true, false, true)]
    [InlineData(true, true, true, false)]
    public void Constructor_NullValues_ThrowsArgumentNullException(bool hasPouleId, bool hasPouleName, bool hasPlayerCount, bool hasRoundId)
    {
        // arrange
        var pouleId = hasPouleId ? new PouleId(Guid.NewGuid()) : null!;
        var pouleName = hasPouleName ? PouleName.Create("abc") : null!;
        var poulePlayersCount = hasPlayerCount ? PoulePlayersCount.Create(4) : null!;
        var roundId = hasRoundId ? new RoundId(Guid.NewGuid()) : null!;

        // act & assert
        Assert.Throws<ArgumentNullException>(() => new Poule(pouleId, pouleName, poulePlayersCount, roundId));
    }

    [Fact]
    public void Rename_ValidValue_ChangesName()
    {
        // arrange
        var pouleId = new PouleId(Guid.NewGuid());
        var pouleName = PouleName.Create("abc");
        var poulePlayersCount = PoulePlayersCount.Create(4);
        var roundId = new RoundId(Guid.NewGuid());
        var poule = new Poule(pouleId, pouleName, poulePlayersCount, roundId);

        var newName = PouleName.Create("def");

        // act
        poule.Rename(newName);

        // assert
        Assert.Multiple(
            () => Assert.Equal(newName, poule.Name)
        );
    }

    [Fact]
    public void Rename_NullValue_ThrowsArgumentNullException()
    {
        // arrange
        var pouleId = new PouleId(Guid.NewGuid());
        var pouleName = PouleName.Create("abc");
        var poulePlayersCount = PoulePlayersCount.Create(4);
        var roundId = new RoundId(Guid.NewGuid());
        var poule = new Poule(pouleId, pouleName, poulePlayersCount, roundId);

        // act & assert
        Assert.Throws<ArgumentNullException>(() => poule.Rename(null!));
    }

    [Fact]
    public void SetTotalPlayers_ValidValue_ChangesTotalPlayers()
    {
        // arrange
        var pouleId = new PouleId(Guid.NewGuid());
        var pouleName = PouleName.Create("abc");
        var poulePlayersCount = PoulePlayersCount.Create(4);
        var roundId = new RoundId(Guid.NewGuid());
        var poule = new Poule(pouleId, pouleName, poulePlayersCount, roundId);

        var totalPlayers = PoulePlayersCount.Create(6);

        // act
        poule.SetTotalPlayers(totalPlayers);

        // assert
        Assert.Multiple(
            () => Assert.Equal(totalPlayers, poule.TotalPlayers)
        );
    }

    [Fact]
    public void SetTotalPlayers_NullValue_ThrowsArgumentNullException()
    {
        // arrange
        var pouleId = new PouleId(Guid.NewGuid());
        var pouleName = PouleName.Create("abc");
        var poulePlayersCount = PoulePlayersCount.Create(4);
        var roundId = new RoundId(Guid.NewGuid());
        var poule = new Poule(pouleId, pouleName, poulePlayersCount, roundId);

        // act
        Assert.Throws<ArgumentNullException>(() => poule.SetTotalPlayers(null!));
    }
}
