using Moq;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class TableTennisMatchTestGenerator
{
    private readonly Mock<PouleTemplateResolver> _mockTemplateResolver;
    private readonly Mock<IPouleService> _mockPouleService;
    private readonly TableTennisMatchGenerator _generator;

    public TableTennisMatchTestGenerator()
    {
        _mockTemplateResolver = new Mock<PouleTemplateResolver>();
        _mockTemplateResolver.Setup(resolve => resolve(It.IsAny<int>())).Returns(new Poule3PlayerTemplateService());
        _mockPouleService = new Mock<IPouleService>();
        _generator = new TableTennisMatchGenerator(_mockTemplateResolver.Object, _mockPouleService.Object);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Generate_Handle_UpdateCalledOnce()
    {
        // arrange
        var player1 = PlayerBuilder.GetSinglePlayer(1);
        var player2 = PlayerBuilder.GetSinglePlayer(2);
        var player3 = PlayerBuilder.GetSinglePlayer(3);

        var poule = PouleBuilder.GetSinglePoule();
        poule.Players = [
            player1,
            player2,
            player3
        ];
        poule.Matches = [];
        
        var round = RoundBuilder.GetSingleRound();
        round.Poules = [poule];
        
        var tournament = TournamentBuilder.GetSingleTournament();
        tournament.Rounds = [round];
        
        Poule updatedPoule = null!;
        _mockPouleService.Setup(service => service.Update(It.IsAny<int>(), It.IsAny<Poule>())).Callback<int, Poule>((id, poule) => { updatedPoule = poule; });

        // act
        _generator.Generate(tournament);

        // assert
        Assert.Multiple(
            () => _mockPouleService.Verify(service => service.Update(It.IsAny<int>(), It.IsAny<Poule>()), Times.Once),
            () => Assert.Equal(poule, updatedPoule),
            () => Assert.NotEqual(player1, updatedPoule.Players[0]),
            () => Assert.NotEqual(player2, updatedPoule.Players[1]),
            () => Assert.NotEqual(player3, updatedPoule.Players[2]),
            () => Assert.Equal(3, updatedPoule.Matches.Count)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Generate_CantHandle_UpdateCalledNever()
    {
        // arrange
        var poule = PouleBuilder.GetSinglePoule();
        poule.Matches = [MatchBuilder.GetSingleMatch()];
        
        var round = RoundBuilder.GetSingleRound();
        round.Poules = [poule];
        
        var tournament = TournamentBuilder.GetSingleTournament();
        tournament.Rounds = [round];

        // act
        _generator.Generate(tournament);

        // assert
        Assert.Multiple(
            () => _mockPouleService.Verify(service => service.Update(It.IsAny<int>(), It.IsAny<Poule>()), Times.Never)
        );
    }
}
