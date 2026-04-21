using Design.Domain.ValueObjects;
using Xunit;

namespace Test.Design.Domain.ValueObjects;

public class UT_TableTennisRoundSettings
{
    [Fact]
    public void Create_ValidValue_ReturnsTableTennisRoundSettings()
    {
        // arrange
        var bestOf = 5;

        // act
        var value = TableTennisRoundSettings.Create(bestOf);

        // assert
        Assert.Multiple(
            () => Assert.NotNull(value),
            () => Assert.Equal(bestOf, value.BestOf)
        );
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(4)]
    public void Create_InvalidValue_ThrowsArgumentException(int bestOf)
    {
        // arrange

        // act & assert
        Assert.Throws<ArgumentException>(() => TableTennisRoundSettings.Create(bestOf));
    }

    [Theory]
    [InlineData(1, 1, true)]
    [InlineData(3, 3, true)]
    [InlineData(3, 5, false)]
    [InlineData(5, 3, false)]
    public void Compare_TwoValues_IsSame(int a, int b, bool expectedResult)
    {
        // arrange
        var ttrsA = TableTennisRoundSettings.Create(a);
        var ttrsB = TableTennisRoundSettings.Create(b);

        // act & assert
        Assert.Multiple(
            () => Assert.Equal(expectedResult, ttrsA == ttrsB),
            () => Assert.Equal(expectedResult, ttrsA.Equals(ttrsB))
        );
    }
}
