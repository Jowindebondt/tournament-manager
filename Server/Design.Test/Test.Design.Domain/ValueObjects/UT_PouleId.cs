using Design.Domain.ValueObjects;
using Xunit;

namespace Test.Design.Domain.ValueObjects;

public class UT_PouleId
{
    [Fact]
    public void Create_ValidValue_InitializesPouleId()
    {
        // arrange
        var guid = Guid.NewGuid();

        // act
        var value = new PouleId(guid);

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
        var idA = new PouleId(guid);
        var idB = new PouleId(guid);

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
        var idA = new PouleId(Guid.NewGuid());
        var idB = new PouleId(Guid.NewGuid());

        // act & assert
        Assert.Multiple(
            () => Assert.False(idA == idB),
            () => Assert.False(idA.Equals(idB))
        );
    }
}
