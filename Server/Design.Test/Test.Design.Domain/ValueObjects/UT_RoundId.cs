using Design.Domain.ValueObjects;
using Xunit;

namespace Test.Design.Domain.ValueObjects;

public class UT_RoundId
{
    [Fact]
    public void Create_ValidValue_InitializesRoundId()
    {
        // arrange
        var guid = Guid.NewGuid();

        // act
        var value = new RoundId(guid);

        // assert
        Assert.Multiple(
            () => Assert.NotNull(value),
            () => Assert.Equal(guid, value.Value)
        );
    }

    [Fact]
    public void Compare_TwoEqualValues_IsSame()
    {
        // arrange
        var guid = Guid.NewGuid();
        var idA = new RoundId(guid);
        var idB = new RoundId(guid);

        // act & assert
        Assert.Multiple(
            () => Assert.True(idA == idB),
            () => Assert.True(idA.Equals(idB))
        );
    }

    [Fact]
    public void Compare_TwoDifferentValues_IsNotSame()
    {
        // arrange
        var idA = new RoundId(Guid.NewGuid());
        var idB = new RoundId(Guid.NewGuid());

        // act & assert
        Assert.Multiple(
            () => Assert.False(idA == idB),
            () => Assert.False(idA.Equals(idB))
        );
    }
}
