using Design.Domain.Entities;
using Design.Domain.Enums;
using Design.Domain.ValueObjects;
using Xunit;

namespace Test.Design.Domain;

public class UT_Entities
{
    private sealed class StubRoundSettings : RoundSettings
    {
        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield break;
        }
    }

    // Tournament
    [Fact]
    public void Tournament_Constructor_ValidArgs_CreatesInstance()
    {
        var id = new TournamentId(Guid.NewGuid());
        var name = TournamentName.Create("Tournament A");

        var tournament = new Tournament(id, name, Sport.TableTennis);

        Assert.Multiple(
            () => Assert.Equal(id, tournament.Id),
            () => Assert.Equal(name, tournament.Name),
            () => Assert.Equal(Sport.TableTennis, tournament.Sport)
        );
    }

    [Fact]
    public void Tournament_Constructor_NullId_ThrowsArgumentNullException()
    {
        var name = TournamentName.Create("Tournament A");
        Assert.Throws<ArgumentNullException>(() => new Tournament(null!, name, Sport.TableTennis));
    }

    [Fact]
    public void Tournament_Constructor_NullName_ThrowsArgumentNullException()
    {
        var id = new TournamentId(Guid.NewGuid());
        Assert.Throws<ArgumentNullException>(() => new Tournament(id, null!, Sport.TableTennis));
    }

    [Fact]
    public void Tournament_Rename_ValidName_UpdatesName()
    {
        var tournament = new Tournament(new TournamentId(Guid.NewGuid()), TournamentName.Create("Old Name"), Sport.TableTennis);
        var newName = TournamentName.Create("New Name");

        tournament.Rename(newName);

        Assert.Equal(newName, tournament.Name);
    }

    [Fact]
    public void Tournament_Rename_NullName_ThrowsArgumentNullException()
    {
        var tournament = new Tournament(new TournamentId(Guid.NewGuid()), TournamentName.Create("Tournament"), Sport.TableTennis);

        Assert.Throws<ArgumentNullException>(() => tournament.Rename(null!));
    }

    // Round
    [Fact]
    public void Round_Constructor_ValidArgs_CreatesInstance()
    {
        var id = new RoundId(Guid.NewGuid());
        var name = RoundName.Create("Round A");
        var tournamentId = new TournamentId(Guid.NewGuid());

        var round = new Round(id, name, tournamentId);

        Assert.Multiple(
            () => Assert.Equal(id, round.Id),
            () => Assert.Equal(name, round.Name),
            () => Assert.Equal(tournamentId, round.TournamentId)
        );
    }

    [Fact]
    public void Round_Constructor_NullId_ThrowsArgumentNullException()
    {
        var name = RoundName.Create("Round A");
        var tournamentId = new TournamentId(Guid.NewGuid());

        Assert.Throws<ArgumentNullException>(() => new Round(null!, name, tournamentId));
    }

    [Fact]
    public void Round_Constructor_NullName_ThrowsArgumentNullException()
    {
        var id = new RoundId(Guid.NewGuid());
        var tournamentId = new TournamentId(Guid.NewGuid());

        Assert.Throws<ArgumentNullException>(() => new Round(id, null!, tournamentId));
    }

    [Fact]
    public void Round_Constructor_NullTournamentId_ThrowsArgumentNullException()
    {
        var id = new RoundId(Guid.NewGuid());
        var name = RoundName.Create("Round A");

        Assert.Throws<ArgumentNullException>(() => new Round(id, name, null!));
    }

    [Fact]
    public void Round_Rename_ValidName_UpdatesName()
    {
        var round = new Round(new RoundId(Guid.NewGuid()), RoundName.Create("Old Name"), new TournamentId(Guid.NewGuid()));
        var newName = RoundName.Create("New Name");

        round.Rename(newName);

        Assert.Equal(newName, round.Name);
    }

    [Fact]
    public void Round_Rename_NullName_ThrowsArgumentNullException()
    {
        var round = new Round(new RoundId(Guid.NewGuid()), RoundName.Create("Round"), new TournamentId(Guid.NewGuid()));

        Assert.Throws<ArgumentNullException>(() => round.Rename(null!));
    }

    [Fact]
    public void Round_SetPreviousRound_SetsPreviousRoundAndNextRoundLinks()
    {
        var tournamentId = new TournamentId(Guid.NewGuid());
        var round = new Round(new RoundId(Guid.NewGuid()), RoundName.Create("Round"), tournamentId);
        var previousRound = new Round(new RoundId(Guid.NewGuid()), RoundName.Create("Previous Round"), tournamentId);

        round.SetPreviousRound(previousRound);

        Assert.Multiple(
            () => Assert.Equal(previousRound, round.PreviousRound),
            () => Assert.Equal(round, previousRound.NextRound)
        );
    }

    [Fact]
    public void Round_SetPreviousRound_ReplacesExistingPreviousRound_DisconnectsOldLink()
    {
        var tournamentId = new TournamentId(Guid.NewGuid());
        var round = new Round(new RoundId(Guid.NewGuid()), RoundName.Create("Round"), tournamentId);
        var oldPreviousRound = new Round(new RoundId(Guid.NewGuid()), RoundName.Create("Old Previous"), tournamentId);
        var newPreviousRound = new Round(new RoundId(Guid.NewGuid()), RoundName.Create("New Previous"), tournamentId);

        round.SetPreviousRound(oldPreviousRound);
        round.SetPreviousRound(newPreviousRound);

        Assert.Multiple(
            () => Assert.Equal(newPreviousRound, round.PreviousRound),
            () => Assert.Null(oldPreviousRound.NextRound),
            () => Assert.Equal(round, newPreviousRound.NextRound)
        );
    }

    [Fact]
    public void Round_SetSettings_ValidSettings_UpdatesSettings()
    {
        var round = new Round(new RoundId(Guid.NewGuid()), RoundName.Create("Round"), new TournamentId(Guid.NewGuid()));
        var settings = new StubRoundSettings();

        round.SetSettings(settings);

        Assert.Equal(settings, round.Settings);
    }

    [Fact]
    public void Round_SetSettings_NullSettings_ThrowsArgumentNullException()
    {
        var round = new Round(new RoundId(Guid.NewGuid()), RoundName.Create("Round"), new TournamentId(Guid.NewGuid()));

        Assert.Throws<ArgumentNullException>(() => round.SetSettings(null!));
    }

    [Fact]
    public void Round_SetType_RoundRobinType_UpdatesType()
    {
        var round = new Round(new RoundId(Guid.NewGuid()), RoundName.Create("Round"), new TournamentId(Guid.NewGuid()));
        var type = new RoundRobinType();

        round.SetType(type);

        Assert.Equal(type, round.Type);
    }

    [Fact]
    public void Round_SetType_KnockOutType_UpdatesType()
    {
        var round = new Round(new RoundId(Guid.NewGuid()), RoundName.Create("Round"), new TournamentId(Guid.NewGuid()));
        var type = new KnockOutType(KnockOutPhase.Final);

        round.SetType(type);

        Assert.Equal(type, round.Type);
    }

    [Fact]
    public void Round_SetType_NullType_ThrowsArgumentNullException()
    {
        var round = new Round(new RoundId(Guid.NewGuid()), RoundName.Create("Round"), new TournamentId(Guid.NewGuid()));

        Assert.Throws<ArgumentNullException>(() => round.SetType(null!));
    }

    [Fact]
    public void Round_Type_DefaultsToNull()
    {
        var round = new Round(new RoundId(Guid.NewGuid()), RoundName.Create("Round"), new TournamentId(Guid.NewGuid()));

        Assert.Null(round.Type);
    }

    // Poule
    [Fact]
    public void Poule_Constructor_ValidArgs_CreatesInstance()
    {
        var id = new PouleId(Guid.NewGuid());
        var name = PouleName.Create("Poule A");
        var totalPlayers = PoulePlayersCount.Create(4);
        var roundId = new RoundId(Guid.NewGuid());

        var poule = new Poule(id, name, totalPlayers, roundId);

        Assert.Multiple(
            () => Assert.Equal(id, poule.Id),
            () => Assert.Equal(name, poule.Name),
            () => Assert.Equal(totalPlayers, poule.TotalPlayers),
            () => Assert.Equal(roundId, poule.RoundId)
        );
    }

    [Fact]
    public void Poule_Constructor_NullId_ThrowsArgumentNullException()
    {
        var name = PouleName.Create("Poule A");
        var totalPlayers = PoulePlayersCount.Create(4);
        var roundId = new RoundId(Guid.NewGuid());

        Assert.Throws<ArgumentNullException>(() => new Poule(null!, name, totalPlayers, roundId));
    }

    [Fact]
    public void Poule_Constructor_NullName_ThrowsArgumentNullException()
    {
        var id = new PouleId(Guid.NewGuid());
        var totalPlayers = PoulePlayersCount.Create(4);
        var roundId = new RoundId(Guid.NewGuid());

        Assert.Throws<ArgumentNullException>(() => new Poule(id, null!, totalPlayers, roundId));
    }

    [Fact]
    public void Poule_Constructor_NullTotalPlayers_ThrowsArgumentNullException()
    {
        var id = new PouleId(Guid.NewGuid());
        var name = PouleName.Create("Poule A");
        var roundId = new RoundId(Guid.NewGuid());

        Assert.Throws<ArgumentNullException>(() => new Poule(id, name, null!, roundId));
    }

    [Fact]
    public void Poule_Constructor_NullRoundId_ThrowsArgumentNullException()
    {
        var id = new PouleId(Guid.NewGuid());
        var name = PouleName.Create("Poule A");
        var totalPlayers = PoulePlayersCount.Create(4);

        Assert.Throws<ArgumentNullException>(() => new Poule(id, name, totalPlayers, null!));
    }

    [Fact]
    public void Poule_Rename_ValidName_UpdatesName()
    {
        var poule = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("Old Name"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var newName = PouleName.Create("New Name");

        poule.Rename(newName);

        Assert.Equal(newName, poule.Name);
    }

    [Fact]
    public void Poule_Rename_NullName_ThrowsArgumentNullException()
    {
        var poule = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("Poule"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));

        Assert.Throws<ArgumentNullException>(() => poule.Rename(null!));
    }

    [Fact]
    public void Poule_SetTotalPlayers_ValidValue_UpdatesTotalPlayers()
    {
        var poule = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("Poule"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var newTotalPlayers = PoulePlayersCount.Create(8);

        poule.SetTotalPlayers(newTotalPlayers);

        Assert.Equal(newTotalPlayers, poule.TotalPlayers);
    }

    [Fact]
    public void Poule_SetTotalPlayers_NullValue_ThrowsArgumentNullException()
    {
        var poule = new Poule(new PouleId(Guid.NewGuid()), PouleName.Create("Poule"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));

        Assert.Throws<ArgumentNullException>(() => poule.SetTotalPlayers(null!));
    }
}
