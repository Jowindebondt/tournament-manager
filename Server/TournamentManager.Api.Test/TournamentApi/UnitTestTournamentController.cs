using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TournamentManager.Application;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Api.Test.TournamentApi;

public class UnitTestTournamentController
{
    private readonly Mock<ITournamentService> _mockService;
    private readonly TournamentController _controller;

    public UnitTestTournamentController()
    {
        _mockService = new Mock<ITournamentService>();
        _controller = new TournamentController(_mockService.Object);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetList_ReturnsOkWithFilledList_ServiceGetAllCalledOnce()
    {
        // arrange
        _mockService.Setup(service => service.GetAll()).Returns(TournamentBuilder.GetListTournament(5));
        OkObjectResult okResult = null;
        IEnumerable<Tournament> content = null;

        // act
        var result = _controller.GetList();

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.GetAll(), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => content = Assert.IsAssignableFrom<IEnumerable<Tournament>>(okResult.Value),
            () => Assert.Equal(5, content.Count())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetList_ReturnsOkWithEmptyList_ServiceGetAllCalledOnce()
    {
        // arrange
        _mockService.Setup(service => service.GetAll()).Returns((IEnumerable<Tournament>)null);
        OkObjectResult okResult = null;

        // act
        var result = _controller.GetList();

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.GetAll(), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.Null(okResult.Value)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetById_ReturnsOkWithEntity_ServiceGetCalledOnce()
    {
        // arrange
        _mockService.Setup(service => service.Get(It.IsAny<int>())).Returns(TournamentBuilder.GetSingleTournament());
        OkObjectResult okResult = null;

        // act
        var result = _controller.GetById(-1);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.Get(It.IsAny<int>()), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => Assert.IsType<Tournament>(okResult.Value)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetById_ReturnsNotFound_ServiceGetCalledOnce()
    {
        // arrange
        _mockService.Setup(service => service.Get(It.IsAny<int>())).Returns((Tournament)null);

        // act
        var result = _controller.GetById(-1);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.Get(It.IsAny<int>()), Times.Once),
            () => Assert.IsType<NotFoundResult>(result)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void CreateValidInstance_ReturnsOkWithEntity_ServiceInsertCalledOnce()
    {
        // arrange
        var newInstance = TournamentBuilder.GetSingleTournament();

        _mockService.Setup(service => service.Insert(It.IsAny<Tournament>())).Callback(() => { });
        OkObjectResult okResult = null;

        // act
        var result = _controller.Create(newInstance);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.Insert(It.IsAny<Tournament>()), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => Assert.IsType<Tournament>(okResult.Value)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void CreateNoInstance_ReturnsBadRequest_ServiceInsertCalledNever()
    {
        // arrange
        BadRequestObjectResult badRequestResult = null;
        string content = null;

        // act
        var result = _controller.Create(null);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.Insert(It.IsAny<Tournament>()), Times.Never),
            () => badRequestResult = Assert.IsType<BadRequestObjectResult>(result),
            () => Assert.NotNull(badRequestResult.Value),
            () => content = Assert.IsType<string>(badRequestResult.Value),
            () => Assert.False(string.IsNullOrEmpty(content))
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void UpdateValidInstance_ReturnsOkWithEntity_ServiceUpdateCalledOnce()
    {
        // arrange
        _mockService.Setup(service => service.Update(It.IsAny<int>(), It.IsAny<Tournament>())).Returns(TournamentBuilder.GetSingleTournament());
        OkObjectResult okResult = null;

        // act
        var result = _controller.Update(-1, TournamentBuilder.GetSingleTournament());

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.Update(It.IsAny<int>(), It.IsAny<Tournament>()), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => Assert.IsType<Tournament>(okResult.Value)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void UpdateNoInstance_ReturnsBadRequest_ServiceUpdateCalledNever()
    {
        // arrange
        BadRequestObjectResult badRequestResult = null;
        string content = null;

        // act
        var result = _controller.Update(-1, null);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.Update(It.IsAny<int>(), It.IsAny<Tournament>()), Times.Never),
            () => badRequestResult = Assert.IsType<BadRequestObjectResult>(result),
            () => Assert.NotNull(badRequestResult.Value),
            () => content = Assert.IsType<string>(badRequestResult.Value),
            () => Assert.False(string.IsNullOrEmpty(content))
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Delete_ReturnsOk_ServiceDeleteCalledOnce()
    {
        // arrange
        _mockService.Setup(service => service.Delete(It.IsAny<int>())).Callback(() => { });

        // act
        var result = _controller.Delete(-1);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.Delete(It.IsAny<int>()), Times.Once),
            () => Assert.IsType<OkResult>(result)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Generate_ReturnsOk_ServiceGenerateCalledOnce()
    {
        // arrange
        _mockService.Setup(service => service.Generate(It.IsAny<int>())).Callback(() => { });

        // act
        var result = _controller.Generate(-1);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.Generate(It.IsAny<int>()), Times.Once),
            () => Assert.IsType<OkResult>(result)
        );
    }
}