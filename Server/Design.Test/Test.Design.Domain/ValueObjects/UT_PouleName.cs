using Design.Domain.ValueObjects;
using Xunit;

namespace Test.Design.Domain.ValueObjects;

public class UT_PouleName
{
    [Fact]
    public void Create_ValidValue_ReturnsPouleName()
    {
        // arrange
        var str = "abc";

        // act
        var value = PouleName.Create(str);

        // assert
        Assert.Multiple(
            () => Assert.NotNull(value),
            () => Assert.Equal(str, value.Value)
        );
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void Create_InvalidValue_ThrowsArgumentException(int stringLength)
    {
        // arrange
        var str = new string('a', stringLength);

        // act & assert
        Assert.Throws<ArgumentException>(() => PouleName.Create(str));
    }

    [Theory]
    [InlineData("abc", "abc", true)]
    [InlineData("abc", "def", false)]
    [InlineData("def", "abc", false)]
    public void Compare_TwoValues_IsSame(string a, string b, bool expectedResult)
    {
        // arrange
        var tnA = PouleName.Create(a);
        var tnB = PouleName.Create(b);

        // act & assert
        Assert.Multiple(
            () => Assert.Equal(expectedResult, tnA == tnB),
            () => Assert.Equal(expectedResult, tnA.Equals(tnB))
        );
    }
}
