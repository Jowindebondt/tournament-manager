using Moq;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class TableTennisRoundTestGenerator
{
    private readonly Mock<IRoundService> _mockRoundService;
    private readonly TableTennisRoundGenerator _generator;

    public TableTennisRoundTestGenerator()
    {
        _mockRoundService = new Mock<IRoundService>();
        _generator = new TableTennisRoundGenerator(_mockRoundService.Object);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Generate_Handle_InsertAndSetSettingsCalledOnce()
    {
        // arrange
        var tournament = TournamentBuilder.GetSingleTournament();
        tournament.Rounds = [];
        _mockRoundService.Setup(service => service.Insert(It.IsAny<Round>())).Callback<Round>(round => {round.Id = -1;});
        _mockRoundService.Setup(service => service.SetSettings(It.IsAny<TableTennisRoundSettings>())).Callback(() => {});

        // act
        _generator.Generate(tournament);

        // assert
        Assert.Multiple(
            () => _mockRoundService.Verify(service => service.Insert(It.IsAny<Round>()), Times.Once),
            () => _mockRoundService.Verify(service => service.SetSettings(It.IsAny<TableTennisRoundSettings>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Generate_CantHandle_InsertAndSetSettingsCalledNever()
    {
        // arrange
        var tournament = TournamentBuilder.GetSingleTournament();
        tournament.Rounds = [RoundBuilder.GetSingleRound()];

        // act
        _generator.Generate(tournament);

        // assert
        Assert.Multiple(
            () => _mockRoundService.Verify(service => service.Insert(It.IsAny<Round>()), Times.Never),
            () => _mockRoundService.Verify(service => service.SetSettings(It.IsAny<TableTennisRoundSettings>()), Times.Never)
        );
    }
}
