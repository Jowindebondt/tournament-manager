using Microsoft.AspNetCore.Mvc;
using Moq;
using TournamentManager.Application;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Api.Test;

public class UnitTestTableTennisController
{
    private readonly Mock<IRoundService> _mockRoundService;
    private readonly Mock<ITournamentService> _mockTournamentService;
    private readonly TableTennisController _controller;

    public UnitTestTableTennisController()
    {
        _mockRoundService = new Mock<IRoundService>();
        _mockTournamentService = new Mock<ITournamentService>();
        _controller = new TableTennisController(_mockTournamentService.Object, _mockRoundService.Object);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void SetTournamentSettings_ReturnsOk_ServiceSetSettingsCalledOnce()
    {
        // arrange
        _mockTournamentService.Setup(service => service.SetSettings(It.IsAny<TournamentSettings>())).Callback(() => { });

        // act
        var result = _controller.SetTournamentSettings(TournamentSettingsBuilder.GetSingleTournamentSettings<TableTennisSettings>());

        // assert
        Assert.Multiple(
            () => _mockTournamentService.Verify(service => service.SetSettings(It.IsAny<TournamentSettings>()), Times.Once),
            () => Assert.IsType<OkResult>(result)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void SetTournamentSettings_ReturnsBadRequest_ServiceSetSettingsCalledNever()
    {
        // arrange
        BadRequestObjectResult badRequestResult = null!;
        string content = null!;
        
        // act
        var result = _controller.SetTournamentSettings(null!);

        // assert
        Assert.Multiple(
            () => _mockTournamentService.Verify(service => service.SetSettings(It.IsAny<TournamentSettings>()), Times.Never),
            () => badRequestResult = Assert.IsType<BadRequestObjectResult>(result),
            () => Assert.NotNull(badRequestResult.Value),
            () => content = Assert.IsType<string>(badRequestResult.Value),
            () => Assert.False(string.IsNullOrEmpty(content))
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void SetRoundSettings_ReturnsOk_ServiceSetSettingsCalledOnce()
    {
        // arrange
        _mockRoundService.Setup(service => service.SetSettings(It.IsAny<RoundSettings>())).Callback(() => { });

        // act
        var result = _controller.SetRoundSettings(RoundSettingsBuilder.GetSingleRoundSettings<TableTennisRoundSettings>());

        // assert
        Assert.Multiple(
            () => _mockRoundService.Verify(service => service.SetSettings(It.IsAny<RoundSettings>()), Times.Once),
            () => Assert.IsType<OkResult>(result)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void SetRoundSettings_ReturnsBadRequest_ServiceSetSettingsCalledNever()
    {
        // arrange
        BadRequestObjectResult badRequestResult = null!;
        string content = null!;
        
        // act
        var result = _controller.SetRoundSettings(null!);

        // assert
        Assert.Multiple(
            () => _mockRoundService.Verify(service => service.SetSettings(It.IsAny<RoundSettings>()), Times.Never),
            () => badRequestResult = Assert.IsType<BadRequestObjectResult>(result),
            () => Assert.NotNull(badRequestResult.Value),
            () => content = Assert.IsType<string>(badRequestResult.Value),
            () => Assert.False(string.IsNullOrEmpty(content))
        );
    }
}
