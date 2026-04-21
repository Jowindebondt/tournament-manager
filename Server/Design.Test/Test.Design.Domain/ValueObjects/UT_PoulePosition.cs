using Design.Domain.Entities;
using Design.Domain.ValueObjects;
using Xunit;

namespace Test.Design.Domain.ValueObjects;

public class UT_PoulePosition
{
    [Fact]
    public void Create_ValidValue_ReturnsPoulePosition()
    {
        // arrange
        var poule = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("abc"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var position = 2;

        // act
        var value = PoulePosition.Create(poule, position);

        // assert
        Assert.Multiple(
            () => Assert.NotNull(value),
            () => Assert.Equal(position, value.Position),
            () => Assert.Equal(poule, value.Poule)
        );
    }

    [Fact]
    public void Create_PouleIsNull_ThrowsArgumentNullException()
    {
        // arrange
        var position = 2;

        // act & assert
        Assert.Throws<ArgumentNullException>(() => PoulePosition.Create(null!, position));
    }

    [Theory]
    [InlineData(4, 0)]
    [InlineData(4, 5)]
    public void Create_InvalidPosition_ThrowsArgumentException(int poulePlayersCount, int position)
    {
        // arrange
        var poule = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("abc"), PoulePlayersCount.Create(poulePlayersCount), new RoundId(Guid.NewGuid()));

        // act & assert
        Assert.Throws<ArgumentException>(() => PoulePosition.Create(poule, position));
    }

    [Fact]
    public void Compare_SameValues_IsSame()
    {
        // arrange
        var poule = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("abc"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var ppA = PoulePosition.Create(poule, 2);
        var ppB = PoulePosition.Create(poule, 2);
        
        // act & assert
        Assert.Multiple(
            () => Assert.True(ppA == ppB),
            () => Assert.True(ppA.Equals(ppB))
        );
    }

    [Fact]
    public void Compare_SamePouleDifferentPosition_IsNotSame()
    {
        // arrange
        var poule = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("abc"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var ppA = PoulePosition.Create(poule, 2);
        var ppB = PoulePosition.Create(poule, 3);
        
        // act & assert
        Assert.Multiple(
            () => Assert.False(ppA == ppB),
            () => Assert.False(ppA.Equals(ppB))
        );
    }

    [Fact]
    public void Compare_DifferentPouleSamePosition_IsNotSame()
    {
        // arrange
        var pouleA = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("abc"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var pouleB = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("def"), PoulePlayersCount.Create(6), new RoundId(Guid.NewGuid()));
        var ppA = PoulePosition.Create(pouleA, 2);
        var ppB = PoulePosition.Create(pouleB, 2);
        
        // act & assert
        Assert.Multiple(
            () => Assert.False(ppA == ppB),
            () => Assert.False(ppA.Equals(ppB))
        );
    }
}
