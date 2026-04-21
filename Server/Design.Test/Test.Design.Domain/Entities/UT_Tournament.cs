using Design.Domain.Entities;
using Design.Domain.Enums;
using Design.Domain.ValueObjects;
using Xunit;

namespace Test.Design.Domain.Entities;

public class UT_Tournament
{
    [Fact]
    public void Constructor_ValidValues_InitializesTournament()
    {
        // arrange
        var tournamentId = new TournamentId(Guid.NewGuid());
        var tournamentName = TournamentName.Create("abc");

        // act
        var value = new Tournament(tournamentId, tournamentName, Sport.TableTennis);

        // assert
        Assert.Multiple(
            () => Assert.NotNull(value),
            () => Assert.Equal(tournamentId, value.Id),
            () => Assert.Equal(tournamentName, value.Name),
            () => Assert.Equal(Sport.TableTennis, value.Sport)
        );
    }

    [Theory]
    [InlineData(false, true)]
    [InlineData(true, false)]
    public void Constructor_NullValues_ThrowsArgumentNullException(bool hasTournamentId, bool hasTournamentName)
    {
        // arrange
        var tournamentId = hasTournamentId ? new TournamentId(Guid.NewGuid()) : null!;
        var tournamentName = hasTournamentName ? TournamentName.Create("abc") : null!;

        // act & assert
        Assert.Throws<ArgumentNullException>(() => new Tournament(tournamentId, tournamentName, Sport.TableTennis));
    }

    [Fact]
    public void Rename_ValidValue_ChangesName()
    {
        // arrange
        var tournamentId = new TournamentId(Guid.NewGuid());
        var tournamentName = TournamentName.Create("abc");
        var tournament = new Tournament(tournamentId, tournamentName, Sport.TableTennis);
        var newName = TournamentName.Create("def");

        // act
        tournament.Rename(newName);

        // assert
        Assert.Multiple(
            () => Assert.Equal(newName, tournament.Name)
        );
    }

    [Fact]
    public void Rename_NullValue_ThrowsArgumentNullException()
    {
        // arrange
        var tournamentId = new TournamentId(Guid.NewGuid());
        var tournamentName = TournamentName.Create("abc");
        var tournament = new Tournament(tournamentId, tournamentName, Sport.TableTennis);

        // act & assert
        Assert.Throws<ArgumentNullException>(() => tournament.Rename(null!));
    }
}
