using Design.Domain.Entities;
using Design.Domain.ValueObjects;
using Xunit;

namespace Test.Design.Domain;

public class UT_ValueObjects
{
    // TournamentName
    [Fact]
    public void TournamentName_Create_ValidValue_ReturnsInstance()
    {
        var name = TournamentName.Create("Tournament A");
        Assert.Equal("Tournament A", name.Value);
    }

    [Fact]
    public void TournamentName_Create_EmptyValue_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => TournamentName.Create(string.Empty));
    }

    [Fact]
    public void TournamentName_Create_TooLongValue_ThrowsArgumentException()
    {
        var tooLong = new string('a', TournamentName.MaxLength + 1);
        Assert.Throws<ArgumentException>(() => TournamentName.Create(tooLong));
    }

    [Fact]
    public void TournamentName_Create_MaxLengthValue_ReturnsInstance()
    {
        var maxLength = new string('a', TournamentName.MaxLength);
        var name = TournamentName.Create(maxLength);
        Assert.Equal(maxLength, name.Value);
    }

    // RoundName
    [Fact]
    public void RoundName_Create_ValidValue_ReturnsInstance()
    {
        var name = RoundName.Create("Round A");
        Assert.Equal("Round A", name.Value);
    }

    [Fact]
    public void RoundName_Create_EmptyValue_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => RoundName.Create(string.Empty));
    }

    [Fact]
    public void RoundName_Create_TooLongValue_ThrowsArgumentException()
    {
        var tooLong = new string('a', RoundName.MaxLength + 1);
        Assert.Throws<ArgumentException>(() => RoundName.Create(tooLong));
    }

    [Fact]
    public void RoundName_Create_MaxLengthValue_ReturnsInstance()
    {
        var maxLength = new string('a', RoundName.MaxLength);
        var name = RoundName.Create(maxLength);
        Assert.Equal(maxLength, name.Value);
    }

    // PouleName
    [Fact]
    public void PouleName_Create_ValidValue_ReturnsInstance()
    {
        var name = PouleName.Create("Poule A");
        Assert.Equal("Poule A", name.Value);
    }

    [Fact]
    public void PouleName_Create_EmptyValue_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => PouleName.Create(string.Empty));
    }

    [Fact]
    public void PouleName_Create_TooLongValue_ThrowsArgumentException()
    {
        var tooLong = new string('a', PouleName.MaxLength + 1);
        Assert.Throws<ArgumentException>(() => PouleName.Create(tooLong));
    }

    [Fact]
    public void PouleName_Create_MaxLengthValue_ReturnsInstance()
    {
        var maxLength = new string('a', PouleName.MaxLength);
        var name = PouleName.Create(maxLength);
        Assert.Equal(maxLength, name.Value);
    }

    // PoulePlayersCount
    [Fact]
    public void PoulePlayersCount_Create_MinValue_ReturnsInstance()
    {
        var count = PoulePlayersCount.Create(PoulePlayersCount.MinValue);
        Assert.Equal(PoulePlayersCount.MinValue, count.Value);
    }

    [Fact]
    public void PoulePlayersCount_Create_MaxValue_ReturnsInstance()
    {
        var count = PoulePlayersCount.Create(PoulePlayersCount.MaxValue);
        Assert.Equal(PoulePlayersCount.MaxValue, count.Value);
    }

    [Fact]
    public void PoulePlayersCount_Create_BelowMin_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => PoulePlayersCount.Create((short)(PoulePlayersCount.MinValue - 1)));
    }

    [Fact]
    public void PoulePlayersCount_Create_AboveMax_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => PoulePlayersCount.Create((short)(PoulePlayersCount.MaxValue + 1)));
    }

    // TournamentId
    [Fact]
    public void TournamentId_Constructor_StoresValue()
    {
        var guid = Guid.NewGuid();
        var id = new TournamentId(guid);
        Assert.Equal(guid, id.Value);
    }

    // RoundId
    [Fact]
    public void RoundId_Constructor_StoresValue()
    {
        var guid = Guid.NewGuid();
        var id = new RoundId(guid);
        Assert.Equal(guid, id.Value);
    }

    // PouleId
    [Fact]
    public void PouleId_Constructor_StoresValue()
    {
        var guid = Guid.NewGuid();
        var id = new PouleId(guid);
        Assert.Equal(guid, id.Value);
    }

    // PoulePosition
    [Fact]
    public void PoulePosition_Create_ValidValues_ReturnsInstance()
    {
        var poule = new Poule(
            new PouleId(Guid.NewGuid()),
            PouleName.Create("Poule A"),
            PoulePlayersCount.Create(4),
            new RoundId(Guid.NewGuid()));

        var position = PoulePosition.Create(poule, 1);

        Assert.Multiple(
            () => Assert.Equal(poule, position.Poule),
            () => Assert.Equal(1, position.Position)
        );
    }

    [Fact]
    public void PoulePosition_Create_NullPoule_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => PoulePosition.Create(null!, 1));
    }

    [Fact]
    public void PoulePosition_Create_PositionTooLow_ThrowsArgumentException()
    {
        var poule = new Poule(
            new PouleId(Guid.NewGuid()),
            PouleName.Create("Poule A"),
            PoulePlayersCount.Create(4),
            new RoundId(Guid.NewGuid()));

        Assert.Throws<ArgumentException>(() => PoulePosition.Create(poule, 0));
    }

    [Fact]
    public void PoulePosition_Create_PositionTooHigh_ThrowsArgumentException()
    {
        var poule = new Poule(
            new PouleId(Guid.NewGuid()),
            PouleName.Create("Poule A"),
            PoulePlayersCount.Create(4),
            new RoundId(Guid.NewGuid()));

        Assert.Throws<ArgumentException>(() => PoulePosition.Create(poule, 5));
    }
}
