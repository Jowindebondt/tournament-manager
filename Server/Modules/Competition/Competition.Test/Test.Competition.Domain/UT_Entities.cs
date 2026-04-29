using Competition.Domain.Enums;
using Competition.Domain.ValueObjects;
using Xunit;
using CompetitionEntity = Competition.Domain.Entities.Competition;
using CompetitionPoule = Competition.Domain.Entities.CompetitionPoule;
using CompetitionRound = Competition.Domain.Entities.CompetitionRound;
using Competitor = Competition.Domain.Entities.Competitor;
using RoundRobinGame = Competition.Domain.Entities.RoundRobinGame;
using BracketGame = Competition.Domain.Entities.BracketGame;

namespace Test.Competition.Domain;

public class UT_Entities
{
    // Competition
    [Fact]
    public void Competition_Constructor_ValidArgs_CreatesInstance()
    {
        var id = new CompetitionId(Guid.NewGuid());
        var name = CompetitionName.Create("Competition A");

        var competition = new CompetitionEntity(id, name, Sport.TableTennis);

        Assert.Multiple(
            () => Assert.Equal(id, competition.Id),
            () => Assert.Equal(name, competition.Name),
            () => Assert.Equal(Sport.TableTennis, competition.Sport),
            () => Assert.Equal(CompetitionStatus.Planned, competition.Status)
        );
    }

    [Fact]
    public void Competition_Constructor_NullId_ThrowsArgumentNullException()
    {
        var name = CompetitionName.Create("Competition A");
        Assert.Throws<ArgumentNullException>(() => new CompetitionEntity(null!, name, Sport.TableTennis));
    }

    [Fact]
    public void Competition_Constructor_NullName_ThrowsArgumentNullException()
    {
        var id = new CompetitionId(Guid.NewGuid());
        Assert.Throws<ArgumentNullException>(() => new CompetitionEntity(id, null!, Sport.TableTennis));
    }

    [Fact]
    public void Competition_Rename_ValidName_UpdatesName()
    {
        var competition = new CompetitionEntity(new CompetitionId(Guid.NewGuid()), CompetitionName.Create("Old Name"), Sport.TableTennis);
        var newName = CompetitionName.Create("New Name");

        competition.Rename(newName);

        Assert.Equal(newName, competition.Name);
    }

    [Fact]
    public void Competition_Rename_NullName_ThrowsArgumentNullException()
    {
        var competition = new CompetitionEntity(new CompetitionId(Guid.NewGuid()), CompetitionName.Create("Competition"), Sport.TableTennis);

        Assert.Throws<ArgumentNullException>(() => competition.Rename(null!));
    }

    [Fact]
    public void Competition_Start_FromPlanned_SetsStatusActive()
    {
        var competition = new CompetitionEntity(new CompetitionId(Guid.NewGuid()), CompetitionName.Create("Competition"), Sport.TableTennis);

        competition.Start();

        Assert.Equal(CompetitionStatus.Active, competition.Status);
    }

    [Fact]
    public void Competition_Start_FromActive_ThrowsInvalidOperationException()
    {
        var competition = new CompetitionEntity(new CompetitionId(Guid.NewGuid()), CompetitionName.Create("Competition"), Sport.TableTennis);
        competition.Start();

        Assert.Throws<InvalidOperationException>(() => competition.Start());
    }

    [Fact]
    public void Competition_Complete_FromActive_SetsStatusCompleted()
    {
        var competition = new CompetitionEntity(new CompetitionId(Guid.NewGuid()), CompetitionName.Create("Competition"), Sport.TableTennis);
        competition.Start();

        competition.Complete();

        Assert.Equal(CompetitionStatus.Completed, competition.Status);
    }

    [Fact]
    public void Competition_Complete_FromPlanned_ThrowsInvalidOperationException()
    {
        var competition = new CompetitionEntity(new CompetitionId(Guid.NewGuid()), CompetitionName.Create("Competition"), Sport.TableTennis);

        Assert.Throws<InvalidOperationException>(() => competition.Complete());
    }

    // CompetitionRound
    [Fact]
    public void CompetitionRound_Constructor_ValidArgs_CreatesInstance()
    {
        var id = new CompetitionRoundId(Guid.NewGuid());
        var name = CompetitionRoundName.Create("Round A");
        var competitionId = new CompetitionId(Guid.NewGuid());

        var round = new CompetitionRound(id, name, competitionId);

        Assert.Multiple(
            () => Assert.Equal(id, round.Id),
            () => Assert.Equal(name, round.Name),
            () => Assert.Equal(competitionId, round.CompetitionId)
        );
    }

    [Fact]
    public void CompetitionRound_Constructor_NullId_ThrowsArgumentNullException()
    {
        var name = CompetitionRoundName.Create("Round A");
        var competitionId = new CompetitionId(Guid.NewGuid());

        Assert.Throws<ArgumentNullException>(() => new CompetitionRound(null!, name, competitionId));
    }

    [Fact]
    public void CompetitionRound_Constructor_NullName_ThrowsArgumentNullException()
    {
        var id = new CompetitionRoundId(Guid.NewGuid());
        var competitionId = new CompetitionId(Guid.NewGuid());

        Assert.Throws<ArgumentNullException>(() => new CompetitionRound(id, null!, competitionId));
    }

    [Fact]
    public void CompetitionRound_Constructor_NullCompetitionId_ThrowsArgumentNullException()
    {
        var id = new CompetitionRoundId(Guid.NewGuid());
        var name = CompetitionRoundName.Create("Round A");

        Assert.Throws<ArgumentNullException>(() => new CompetitionRound(id, name, null!));
    }

    [Fact]
    public void CompetitionRound_Rename_ValidName_UpdatesName()
    {
        var round = new CompetitionRound(new CompetitionRoundId(Guid.NewGuid()), CompetitionRoundName.Create("Old Name"), new CompetitionId(Guid.NewGuid()));
        var newName = CompetitionRoundName.Create("New Name");

        round.Rename(newName);

        Assert.Equal(newName, round.Name);
    }

    [Fact]
    public void CompetitionRound_Rename_NullName_ThrowsArgumentNullException()
    {
        var round = new CompetitionRound(new CompetitionRoundId(Guid.NewGuid()), CompetitionRoundName.Create("Round"), new CompetitionId(Guid.NewGuid()));

        Assert.Throws<ArgumentNullException>(() => round.Rename(null!));
    }

    [Fact]
    public void CompetitionRound_SetPlan_RoundRobinPlan_UpdatesPlan()
    {
        var round = new CompetitionRound(new CompetitionRoundId(Guid.NewGuid()), CompetitionRoundName.Create("Round"), new CompetitionId(Guid.NewGuid()));
        var plan = new RoundRobinPlan();

        round.SetPlan(plan);

        Assert.Equal(plan, round.Plan);
    }

    [Fact]
    public void CompetitionRound_SetPlan_KnockOutPlan_UpdatesPlan()
    {
        var round = new CompetitionRound(new CompetitionRoundId(Guid.NewGuid()), CompetitionRoundName.Create("Round"), new CompetitionId(Guid.NewGuid()));
        var plan = new KnockOutPlan(KnockOutPhase.Final);

        round.SetPlan(plan);

        Assert.Equal(plan, round.Plan);
    }

    [Fact]
    public void CompetitionRound_SetPlan_NullPlan_ThrowsArgumentNullException()
    {
        var round = new CompetitionRound(new CompetitionRoundId(Guid.NewGuid()), CompetitionRoundName.Create("Round"), new CompetitionId(Guid.NewGuid()));

        Assert.Throws<ArgumentNullException>(() => round.SetPlan(null!));
    }

    // CompetitionPoule
    [Fact]
    public void CompetitionPoule_Constructor_ValidArgs_CreatesInstance()
    {
        var id = new CompetitionPouleId(Guid.NewGuid());
        var name = CompetitionPouleName.Create("Poule A");
        var roundId = new CompetitionRoundId(Guid.NewGuid());

        var poule = new CompetitionPoule(id, name, roundId);

        Assert.Multiple(
            () => Assert.Equal(id, poule.Id),
            () => Assert.Equal(name, poule.Name),
            () => Assert.Equal(roundId, poule.CompetitionRoundId)
        );
    }

    [Fact]
    public void CompetitionPoule_Constructor_NullId_ThrowsArgumentNullException()
    {
        var name = CompetitionPouleName.Create("Poule A");
        var roundId = new CompetitionRoundId(Guid.NewGuid());

        Assert.Throws<ArgumentNullException>(() => new CompetitionPoule(null!, name, roundId));
    }

    [Fact]
    public void CompetitionPoule_Constructor_NullName_ThrowsArgumentNullException()
    {
        var id = new CompetitionPouleId(Guid.NewGuid());
        var roundId = new CompetitionRoundId(Guid.NewGuid());

        Assert.Throws<ArgumentNullException>(() => new CompetitionPoule(id, null!, roundId));
    }

    [Fact]
    public void CompetitionPoule_Constructor_NullRoundId_ThrowsArgumentNullException()
    {
        var id = new CompetitionPouleId(Guid.NewGuid());
        var name = CompetitionPouleName.Create("Poule A");

        Assert.Throws<ArgumentNullException>(() => new CompetitionPoule(id, name, null!));
    }

    [Fact]
    public void CompetitionPoule_Rename_ValidName_UpdatesName()
    {
        var poule = new CompetitionPoule(new CompetitionPouleId(Guid.NewGuid()), CompetitionPouleName.Create("Old Name"), new CompetitionRoundId(Guid.NewGuid()));
        var newName = CompetitionPouleName.Create("New Name");

        poule.Rename(newName);

        Assert.Equal(newName, poule.Name);
    }

    [Fact]
    public void CompetitionPoule_Rename_NullName_ThrowsArgumentNullException()
    {
        var poule = new CompetitionPoule(new CompetitionPouleId(Guid.NewGuid()), CompetitionPouleName.Create("Poule"), new CompetitionRoundId(Guid.NewGuid()));

        Assert.Throws<ArgumentNullException>(() => poule.Rename(null!));
    }

    // Competitor
    [Fact]
    public void Competitor_Constructor_ValidArgs_CreatesInstance()
    {
        var id = new CompetitorId(Guid.NewGuid());
        var name = CompetitorName.Create("Player A");
        var pouleId = new CompetitionPouleId(Guid.NewGuid());

        var competitor = new Competitor(id, name, pouleId);

        Assert.Multiple(
            () => Assert.Equal(id, competitor.Id),
            () => Assert.Equal(name, competitor.Name),
            () => Assert.Equal(pouleId, competitor.CompetitionPouleId)
        );
    }

    [Fact]
    public void Competitor_Constructor_NullId_ThrowsArgumentNullException()
    {
        var name = CompetitorName.Create("Player A");
        var pouleId = new CompetitionPouleId(Guid.NewGuid());
        Assert.Throws<ArgumentNullException>(() => new Competitor(null!, name, pouleId));
    }

    [Fact]
    public void Competitor_Constructor_NullName_ThrowsArgumentNullException()
    {
        var id = new CompetitorId(Guid.NewGuid());
        var pouleId = new CompetitionPouleId(Guid.NewGuid());
        Assert.Throws<ArgumentNullException>(() => new Competitor(id, null!, pouleId));
    }

    [Fact]
    public void Competitor_Constructor_NullPouleId_ThrowsArgumentNullException()
    {
        var id = new CompetitorId(Guid.NewGuid());
        var name = CompetitorName.Create("Player A");
        Assert.Throws<ArgumentNullException>(() => new Competitor(id, name, null!));
    }

    [Fact]
    public void Competitor_Rename_ValidName_UpdatesName()
    {
        var competitor = new Competitor(new CompetitorId(Guid.NewGuid()), CompetitorName.Create("Old Name"), new CompetitionPouleId(Guid.NewGuid()));
        var newName = CompetitorName.Create("New Name");

        competitor.Rename(newName);

        Assert.Equal(newName, competitor.Name);
    }

    [Fact]
    public void Competitor_Rename_NullName_ThrowsArgumentNullException()
    {
        var competitor = new Competitor(new CompetitorId(Guid.NewGuid()), CompetitorName.Create("Player"), new CompetitionPouleId(Guid.NewGuid()));
        Assert.Throws<ArgumentNullException>(() => competitor.Rename(null!));
    }

    // RoundRobinGame
    [Fact]
    public void RoundRobinGame_Constructor_ValidArgs_CreatesInstance()
    {
        var id = new RoundRobinGameId(Guid.NewGuid());
        var homeId = new CompetitorId(Guid.NewGuid());
        var awayId = new CompetitorId(Guid.NewGuid());
        var pouleId = new CompetitionPouleId(Guid.NewGuid());

        var game = new RoundRobinGame(id, homeId, awayId, pouleId);

        Assert.Multiple(
            () => Assert.Equal(id, game.Id),
            () => Assert.Equal(homeId, game.HomeCompetitorId),
            () => Assert.Equal(awayId, game.AwayCompetitorId),
            () => Assert.Equal(pouleId, game.CompetitionPouleId),
            () => Assert.Null(game.HomeScore),
            () => Assert.Null(game.AwayScore)
        );
    }

    [Fact]
    public void RoundRobinGame_Constructor_NullId_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new RoundRobinGame(null!, new CompetitorId(Guid.NewGuid()), new CompetitorId(Guid.NewGuid()), new CompetitionPouleId(Guid.NewGuid())));
    }

    [Fact]
    public void RoundRobinGame_Constructor_NullHomeCompetitorId_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new RoundRobinGame(new RoundRobinGameId(Guid.NewGuid()), null!, new CompetitorId(Guid.NewGuid()), new CompetitionPouleId(Guid.NewGuid())));
    }

    [Fact]
    public void RoundRobinGame_Constructor_NullAwayCompetitorId_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new RoundRobinGame(new RoundRobinGameId(Guid.NewGuid()), new CompetitorId(Guid.NewGuid()), null!, new CompetitionPouleId(Guid.NewGuid())));
    }

    [Fact]
    public void RoundRobinGame_Constructor_NullPouleId_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new RoundRobinGame(new RoundRobinGameId(Guid.NewGuid()), new CompetitorId(Guid.NewGuid()), new CompetitorId(Guid.NewGuid()), null!));
    }

    [Fact]
    public void RoundRobinGame_SetScore_ValidScores_SetsScores()
    {
        var game = new RoundRobinGame(new RoundRobinGameId(Guid.NewGuid()), new CompetitorId(Guid.NewGuid()), new CompetitorId(Guid.NewGuid()), new CompetitionPouleId(Guid.NewGuid()));

        game.SetScore(3, 1);

        Assert.Multiple(
            () => Assert.Equal(3, game.HomeScore),
            () => Assert.Equal(1, game.AwayScore)
        );
    }

    [Fact]
    public void RoundRobinGame_SetScore_NegativeHomeScore_ThrowsArgumentOutOfRangeException()
    {
        var game = new RoundRobinGame(new RoundRobinGameId(Guid.NewGuid()), new CompetitorId(Guid.NewGuid()), new CompetitorId(Guid.NewGuid()), new CompetitionPouleId(Guid.NewGuid()));
        Assert.Throws<ArgumentOutOfRangeException>(() => game.SetScore(-1, 0));
    }

    [Fact]
    public void RoundRobinGame_SetScore_NegativeAwayScore_ThrowsArgumentOutOfRangeException()
    {
        var game = new RoundRobinGame(new RoundRobinGameId(Guid.NewGuid()), new CompetitorId(Guid.NewGuid()), new CompetitorId(Guid.NewGuid()), new CompetitionPouleId(Guid.NewGuid()));
        Assert.Throws<ArgumentOutOfRangeException>(() => game.SetScore(0, -1));
    }

    [Fact]
    public void RoundRobinGame_ClearScore_AfterSetScore_SetsScoresToNull()
    {
        var game = new RoundRobinGame(new RoundRobinGameId(Guid.NewGuid()), new CompetitorId(Guid.NewGuid()), new CompetitorId(Guid.NewGuid()), new CompetitionPouleId(Guid.NewGuid()));
        game.SetScore(2, 0);

        game.ClearScore();

        Assert.Multiple(
            () => Assert.Null(game.HomeScore),
            () => Assert.Null(game.AwayScore)
        );
    }

    // BracketGame
    [Fact]
    public void BracketGame_Constructor_ValidArgs_CreatesInstance()
    {
        var id = new BracketGameId(Guid.NewGuid());
        var roundId = new CompetitionRoundId(Guid.NewGuid());

        var game = new BracketGame(id, roundId);

        Assert.Multiple(
            () => Assert.Equal(id, game.Id),
            () => Assert.Equal(roundId, game.CompetitionRoundId),
            () => Assert.Null(game.HomeCompetitorId),
            () => Assert.Null(game.AwayCompetitorId),
            () => Assert.Null(game.HomeScore),
            () => Assert.Null(game.AwayScore)
        );
    }

    [Fact]
    public void BracketGame_Constructor_NullId_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new BracketGame(null!, new CompetitionRoundId(Guid.NewGuid())));
    }

    [Fact]
    public void BracketGame_Constructor_NullRoundId_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new BracketGame(new BracketGameId(Guid.NewGuid()), null!));
    }

    [Fact]
    public void BracketGame_SetCompetitors_ValidArgs_SetsCompetitors()
    {
        var game = new BracketGame(new BracketGameId(Guid.NewGuid()), new CompetitionRoundId(Guid.NewGuid()));
        var homeId = new CompetitorId(Guid.NewGuid());
        var awayId = new CompetitorId(Guid.NewGuid());

        game.SetCompetitors(homeId, awayId);

        Assert.Multiple(
            () => Assert.Equal(homeId, game.HomeCompetitorId),
            () => Assert.Equal(awayId, game.AwayCompetitorId)
        );
    }

    [Fact]
    public void BracketGame_SetCompetitors_NullHomeId_ThrowsArgumentNullException()
    {
        var game = new BracketGame(new BracketGameId(Guid.NewGuid()), new CompetitionRoundId(Guid.NewGuid()));
        Assert.Throws<ArgumentNullException>(() => game.SetCompetitors(null!, new CompetitorId(Guid.NewGuid())));
    }

    [Fact]
    public void BracketGame_SetCompetitors_NullAwayId_ThrowsArgumentNullException()
    {
        var game = new BracketGame(new BracketGameId(Guid.NewGuid()), new CompetitionRoundId(Guid.NewGuid()));
        Assert.Throws<ArgumentNullException>(() => game.SetCompetitors(new CompetitorId(Guid.NewGuid()), null!));
    }

    [Fact]
    public void BracketGame_SetScore_ValidScores_SetsScores()
    {
        var game = new BracketGame(new BracketGameId(Guid.NewGuid()), new CompetitionRoundId(Guid.NewGuid()));

        game.SetScore(3, 2);

        Assert.Multiple(
            () => Assert.Equal(3, game.HomeScore),
            () => Assert.Equal(2, game.AwayScore)
        );
    }

    [Fact]
    public void BracketGame_SetScore_NegativeHomeScore_ThrowsArgumentOutOfRangeException()
    {
        var game = new BracketGame(new BracketGameId(Guid.NewGuid()), new CompetitionRoundId(Guid.NewGuid()));
        Assert.Throws<ArgumentOutOfRangeException>(() => game.SetScore(-1, 0));
    }

    [Fact]
    public void BracketGame_SetScore_NegativeAwayScore_ThrowsArgumentOutOfRangeException()
    {
        var game = new BracketGame(new BracketGameId(Guid.NewGuid()), new CompetitionRoundId(Guid.NewGuid()));
        Assert.Throws<ArgumentOutOfRangeException>(() => game.SetScore(0, -1));
    }

    [Fact]
    public void BracketGame_ClearScore_AfterSetScore_SetsScoresToNull()
    {
        var game = new BracketGame(new BracketGameId(Guid.NewGuid()), new CompetitionRoundId(Guid.NewGuid()));
        game.SetScore(1, 0);

        game.ClearScore();

        Assert.Multiple(
            () => Assert.Null(game.HomeScore),
            () => Assert.Null(game.AwayScore)
        );
    }
}
