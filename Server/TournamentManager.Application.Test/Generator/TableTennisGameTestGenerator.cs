using Moq;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class TableTennisGameTestGenerator
{
    private readonly Mock<IGameService> _mockGameService;
    private readonly TableTennisGameGenerator _generator;

    public TableTennisGameTestGenerator()
    {
        _mockGameService = new Mock<IGameService>();
        _generator = new TableTennisGameGenerator(_mockGameService.Object);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Generate_Handle_InsertCalledFiveTimes()
    {
        // arrange
        var match = MatchBuilder.GetSingleMatch();
        match.Games = [];

        var poule = PouleBuilder.GetSinglePoule();
        poule.Matches = [match];
        match.Poule = poule;

        var round = RoundBuilder.GetSingleRound();
        round.Poules = [poule];
        round.Settings = new TableTennisRoundSettings
        {
            BestOf = 5
        };
        poule.Round = round;

        var tournament = TournamentBuilder.GetSingleTournament();
        tournament.Rounds = [round];
        round.Tournament = tournament;
        
        _mockGameService.Setup(service => service.Insert(It.IsAny<Game>())).Callback(() => {});

        // act
        _generator.Generate(tournament);

        // assert
        Assert.Multiple(
            () => _mockGameService.Verify(service => service.Insert(It.IsAny<Game>()), Times.Exactly(5))
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Generate_CantHandle_InsertCalledNever()
    {
        // arrange
        var match = MatchBuilder.GetSingleMatch();
        match.Games = [GameBuilder.GetSingleGame()];

        var poule = PouleBuilder.GetSinglePoule();
        poule.Matches = [match];

        var round = RoundBuilder.GetSingleRound();
        round.Poules = [poule];

        var tournament = TournamentBuilder.GetSingleTournament();
        tournament.Rounds = [round];

        // act
        _generator.Generate(tournament);

        // assert
        Assert.Multiple(
            () => _mockGameService.Verify(service => service.Insert(It.IsAny<Game>()), Times.Never)
        );
    }
}
