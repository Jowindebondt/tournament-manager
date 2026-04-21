using Design.Domain.Entities;
using Design.Domain.ValueObjects;
using Xunit;

namespace Test.Design.Domain.Entities;

public class UT_Round
{
    [Fact]
    public void Constructor_ValidValues_InitializesTournament()
    {
        // arrange
        var roundId = new RoundId(Guid.NewGuid());
        var roundName = RoundName.Create("abc");
        var tournamentId = new TournamentId(Guid.NewGuid());

        // act
        var value = new Round(roundId, roundName, tournamentId);

        // assert
        Assert.Multiple(
            () => Assert.NotNull(value),
            () => Assert.Equal(roundId, value.Id),
            () => Assert.Equal(roundName, value.Name),
            () => Assert.Equal(tournamentId, value.TournamentId)
        );
    }

    [Theory]
    [InlineData(false, true, true)]
    [InlineData(true, false, true)]
    [InlineData(true, true, false)]
    public void Constructor_NullValues_ThrowsArgumentNullException(bool hasRoundId, bool hasRoundName, bool hasTournamentId)
    {
        // arrange
        var roundId = hasRoundId ? new RoundId(Guid.NewGuid()) : null!;
        var roundName = hasRoundName ? RoundName.Create("abc") : null!;
        var tournamentId = hasTournamentId ? new TournamentId(Guid.NewGuid()) : null!;

        // act & assert
        Assert.Throws<ArgumentNullException>(() => new Round(roundId, roundName, tournamentId));
    }

    [Fact]
    public void Rename_ValidValue_ChangesName()
    {
        // arrange
        var roundId = new RoundId(Guid.NewGuid());
        var roundName = RoundName.Create("abc");
        var tournamentId = new TournamentId(Guid.NewGuid());
        var tournament = new Round(roundId, roundName, tournamentId);
        var newName = RoundName.Create("def");

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
        var roundId = new RoundId(Guid.NewGuid());
        var roundName = RoundName.Create("abc");
        var tournamentId = new TournamentId(Guid.NewGuid());
        var tournament = new Round(roundId, roundName, tournamentId);

        // act & assert
        Assert.Throws<ArgumentNullException>(() => tournament.Rename(null!));
    }

    [Fact]
    public void SetPreviousRound_ValidValue_ChangesPreviousRound()
    {
        // arrange
        var roundId = new RoundId(Guid.NewGuid());
        var roundName = RoundName.Create("abc");
        var tournamentId = new TournamentId(Guid.NewGuid());
        var round = new Round(roundId, roundName, tournamentId);

        var prevRoundId = new RoundId(Guid.NewGuid());
        var prevRoundName = RoundName.Create("def");
        var prevRound = new Round(prevRoundId, prevRoundName, tournamentId);

        // act
        round.SetPreviousRound(prevRound);

        // assert
        Assert.Multiple(
            () => Assert.NotNull(round.PreviousRound),
            () => Assert.Equal(prevRound, round.PreviousRound),
            () => Assert.NotNull(prevRound.NextRound),
            () => Assert.Equal(round, prevRound.NextRound)
        );
    }

    [Fact]
    public void SetPreviousRound_ExistingPreviousRoundToNull_ChangesPreviousRound()
    {
        // arrange
        var roundId = new RoundId(Guid.NewGuid());
        var roundName = RoundName.Create("abc");
        var tournamentId = new TournamentId(Guid.NewGuid());
        var round = new Round(roundId, roundName, tournamentId);

        var prevRoundId = new RoundId(Guid.NewGuid());
        var prevRoundName = RoundName.Create("def");
        var prevRound = new Round(prevRoundId, prevRoundName, tournamentId);

        round.SetPreviousRound(prevRound);

        // act
        round.SetPreviousRound(null!);

        // assert
        Assert.Multiple(
            () => Assert.Null(round.PreviousRound),
            () => Assert.Null(prevRound.NextRound)
        );
    }

    [Fact]
    public void SetPreviousRound_ExistingPreviousRoundToDifferentPreviousRound_ChangesPreviousRound()
    {
        // arrange
        var roundId = new RoundId(Guid.NewGuid());
        var roundName = RoundName.Create("abc");
        var tournamentId = new TournamentId(Guid.NewGuid());
        var round = new Round(roundId, roundName, tournamentId);

        var prevRoundId = new RoundId(Guid.NewGuid());
        var prevRoundName = RoundName.Create("def");
        var prevRound = new Round(prevRoundId, prevRoundName, tournamentId);

        round.SetPreviousRound(prevRound);

        var newRoundId = new RoundId(Guid.NewGuid());
        var newRoundName = RoundName.Create("ghi");
        var newRound = new Round(newRoundId, newRoundName, tournamentId);

        // act
        round.SetPreviousRound(newRound);

        // assert
        Assert.Multiple(
            () => Assert.NotNull(round.PreviousRound),
            () => Assert.Equal(newRound, round.PreviousRound),
            () => Assert.Null(prevRound.NextRound),
            () => Assert.NotNull(newRound.NextRound),
            () => Assert.Equal(round, newRound.NextRound)
        );
    }

    [Fact]
    public void SetSettings_ValidValue_ChangesSettings()
    {
        // arrange
        var roundId = new RoundId(Guid.NewGuid());
        var roundName = RoundName.Create("abc");
        var tournamentId = new TournamentId(Guid.NewGuid());
        var round = new Round(roundId, roundName, tournamentId);

        var settings = TableTennisRoundSettings.Create(5);

        // act
        round.SetSettings(settings);

        // assert
        Assert.Multiple(
            () => Assert.NotNull(round.Settings),
            () => Assert.Equal(settings, round.Settings)
        );
    }

    [Fact]
    public void SetSettings_NullValue_ThrowsArgumentNullException()
    {
        // arrange
        var roundId = new RoundId(Guid.NewGuid());
        var roundName = RoundName.Create("abc");
        var tournamentId = new TournamentId(Guid.NewGuid());
        var round = new Round(roundId, roundName, tournamentId);

        // act & assert
        Assert.Throws<ArgumentNullException>(() => round.SetSettings(null!));
    }
}
