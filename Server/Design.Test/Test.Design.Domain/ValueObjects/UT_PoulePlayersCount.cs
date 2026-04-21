using Design.Domain.ValueObjects;
using Xunit;

namespace Test.Design.Domain.ValueObjects;

public class UT_PoulePlayersCount
{
    [Fact]
    public void Create_ValidValue_ReturnsPoulePlayersCount()
    {
        // arrange
        var amount = 8;

        // act
        var value = PoulePlayersCount.Create(amount);

        // assert
        Assert.Multiple(
            () => Assert.NotNull(value),
            () => Assert.Equal(amount, value.Value)
        );
    }

    [Theory]
    [InlineData(2)]
    [InlineData(15)]
    public void Create_InvalidValue_ThrowsArgumentException(int amount)
    {
        // arrange

        // act & assert
        Assert.Throws<ArgumentException>(() => PoulePlayersCount.Create(amount));
    }

    [Theory]
    [InlineData(5, 5, true)]
    [InlineData(5, 6, false)]
    [InlineData(6, 5, false)]
    public void Compare_TwoValues_IsSame(int a, int b, bool expectedResult)
    {
        // arrange
        var tnA = PoulePlayersCount.Create(a);
        var tnB = PoulePlayersCount.Create(b);

        // act & assert
        Assert.Multiple(
            () => Assert.Equal(expectedResult, tnA == tnB),
            () => Assert.Equal(expectedResult, tnA.Equals(tnB))
        );
    }
}
