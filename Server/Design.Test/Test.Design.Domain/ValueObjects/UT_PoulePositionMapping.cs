using Design.Domain.Entities;
using Design.Domain.ValueObjects;
using Xunit;

namespace Test.Design.Domain.ValueObjects;

public class UT_PoulePositionMapping
{
    [Fact]
    public void Create_ValidValue_ReturnsPoulePositionMapping()
    {
        // arrange
        var prevPoule = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("abc"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var prevPosition = 2;
        var prevPoulePosition = PoulePosition.Create(prevPoule, prevPosition);

        var currentPoule = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("def"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var currentPosition = 1;
        var currentPoulePosition = PoulePosition.Create(currentPoule, currentPosition);

        var bestOf = 5;
        var currentRoundSettings = TableTennisRoundSettings.Create(bestOf);

        // act
        var value = PoulePositionMapping.Create(prevPoulePosition, currentPoulePosition, currentRoundSettings);

        // assert
        Assert.Multiple(
            () => Assert.NotNull(value),
            () => Assert.Equal(prevPoulePosition, value.Previous),
            () => Assert.Equal(currentPoulePosition, value.Current),
            () => Assert.Equal(currentRoundSettings, value.RoundSettings)
        );
    }

    [Fact]
    public void Create_PreviousIsNull_ThrowsArgumentNullException()
    {
        // arrange
        var currentPoule = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("def"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var currentPosition = 1;
        var currentPoulePosition = PoulePosition.Create(currentPoule, currentPosition);

        var bestOf = 5;
        var currentRoundSettings = TableTennisRoundSettings.Create(bestOf);

        // act & assert
        Assert.Throws<ArgumentNullException>(() => PoulePositionMapping.Create(null!, currentPoulePosition, currentRoundSettings));
    }

    [Fact]
    public void Create_CurrentIsNull_ThrowsArgumentNullException()
    {
        // arrange
        var prevPoule = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("abc"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var prevPosition = 2;
        var prevPoulePosition = PoulePosition.Create(prevPoule, prevPosition);

        var bestOf = 5;
        var currentRoundSettings = TableTennisRoundSettings.Create(bestOf);

        // act & assert
        Assert.Throws<ArgumentNullException>(() => PoulePositionMapping.Create(prevPoulePosition, null!, currentRoundSettings));
    }

    [Fact]
    public void Create_SettingsIsNull_ThrowsArgumentNullException()
    {
        // arrange
        var prevPoule = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("abc"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var prevPosition = 2;
        var prevPoulePosition = PoulePosition.Create(prevPoule, prevPosition);

        var currentPoule = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("def"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var currentPosition = 1;
        var currentPoulePosition = PoulePosition.Create(currentPoule, currentPosition);

        // act & assert
        Assert.Throws<ArgumentNullException>(() => PoulePositionMapping.Create(prevPoulePosition, currentPoulePosition, null!));
    }

    [Fact]
    public void Compare_SameValues_IsSame()
    {
        // arrange
        var prevPoule = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("abc"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var prevPosition = 2;
        var prevPoulePosition = PoulePosition.Create(prevPoule, prevPosition);

        var currentPoule = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("def"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var currentPosition = 1;
        var currentPoulePosition = PoulePosition.Create(currentPoule, currentPosition);

        var bestOf = 5;
        var currentRoundSettings = TableTennisRoundSettings.Create(bestOf);

        var ppmA = PoulePositionMapping.Create(prevPoulePosition, currentPoulePosition, currentRoundSettings);
        var ppmB = PoulePositionMapping.Create(prevPoulePosition, currentPoulePosition, currentRoundSettings);

        // act & assert
        Assert.Multiple(
            () => Assert.True(ppmA == ppmB),
            () => Assert.True(ppmA.Equals(ppmB))
        );
    }

    [Fact]
    public void Compare_DifferentPrevious_IsNotSame()
    {
        // arrange
        var prevPouleA = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("abc"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var prevPositionA = 2;
        var prevPoulePositionA = PoulePosition.Create(prevPouleA, prevPositionA);

        var prevPouleB = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("xyz"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var prevPositionB = 3;
        var prevPoulePositionB = PoulePosition.Create(prevPouleB, prevPositionB);

        var currentPoule = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("def"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var currentPosition = 1;
        var currentPoulePosition = PoulePosition.Create(currentPoule, currentPosition);

        var bestOf = 5;
        var currentRoundSettings = TableTennisRoundSettings.Create(bestOf);

        var ppmA = PoulePositionMapping.Create(prevPoulePositionA, currentPoulePosition, currentRoundSettings);
        var ppmB = PoulePositionMapping.Create(prevPoulePositionB, currentPoulePosition, currentRoundSettings);

        // act & assert
        Assert.Multiple(
            () => Assert.False(ppmA == ppmB),
            () => Assert.False(ppmA.Equals(ppmB))
        );
    }

    [Fact]
    public void Compare_DifferentCurrent_IsNotSame()
    {
        // arrange
        var prevPoule = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("abc"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var prevPosition = 2;
        var prevPoulePosition = PoulePosition.Create(prevPoule, prevPosition);

        var currentPouleA = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("def"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var currentPositionA = 1;
        var currentPoulePositionA = PoulePosition.Create(currentPouleA, currentPositionA);

        var currentPouleB = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("xyz"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var currentPositionB = 3;
        var currentPoulePositionB = PoulePosition.Create(currentPouleB, currentPositionB);

        var bestOf = 5;
        var currentRoundSettings = TableTennisRoundSettings.Create(bestOf);

        var ppmA = PoulePositionMapping.Create(prevPoulePosition, currentPoulePositionA, currentRoundSettings);
        var ppmB = PoulePositionMapping.Create(prevPoulePosition, currentPoulePositionB, currentRoundSettings);

        // act & assert
        Assert.Multiple(
            () => Assert.False(ppmA == ppmB),
            () => Assert.False(ppmA.Equals(ppmB))
        );
    }

    [Fact]
    public void Compare_DifferentRoundSetting_IsNotSame()
    {
        // arrange
        var prevPoule = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("abc"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var prevPosition = 2;
        var prevPoulePosition = PoulePosition.Create(prevPoule, prevPosition);

        var currentPoule = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("def"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var currentPosition = 1;
        var currentPoulePosition = PoulePosition.Create(currentPoule, currentPosition);

        var bestOfA = 5;
        var currentRoundSettingsA = TableTennisRoundSettings.Create(bestOfA);

        var bestOfB = 3;
        var currentRoundSettingsB = TableTennisRoundSettings.Create(bestOfB);

        var ppmA = PoulePositionMapping.Create(prevPoulePosition, currentPoulePosition, currentRoundSettingsA);
        var ppmB = PoulePositionMapping.Create(prevPoulePosition, currentPoulePosition, currentRoundSettingsB);

        // act & assert
        Assert.Multiple(
            () => Assert.False(ppmA == ppmB),
            () => Assert.False(ppmA.Equals(ppmB))
        );
    }
}
