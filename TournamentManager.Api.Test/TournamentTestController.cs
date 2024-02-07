using Microsoft.AspNetCore.Mvc;
using Moq;
using TournamentManager.Application;
using TournamentManager.Domain;
using Xunit;

namespace TournamentManager.Api.Test;

public class TournamentTestController
{
    private readonly Mock<ITournamentService> _mockService;

    public TournamentTestController()
    {
        _mockService = new Mock<ITournamentService>();
    }

    [Fact]
    public void GetList_ReturnsOkWithFilledList_ServiceGetAllCalledOnce()
    {
        // arrange
        _mockService.Setup(service => service.GetAll()).Returns(GetListTournament(5));
        var controller = new TournamentController(_mockService.Object);
        OkObjectResult okResult = null;
        IEnumerable<Tournament> content = null;

        // act
        var result = controller.GetList();

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
    public void GetList_ReturnsNotFound_ServiceGetAllCalledOnce()
    {
        // arrange
        _mockService.Setup(service => service.GetAll()).Returns((IEnumerable<Tournament>)null);
        var controller = new TournamentController(_mockService.Object);

        // act
        var result = controller.GetList();

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.GetAll(), Times.Once),
            () => Assert.IsType<NotFoundResult>(result)
        );
    }

    [Fact]
    public void GetById_ReturnsOkWithEntity_ServiceGetCalledOnce()
    {
        // arrange
        _mockService.Setup(service => service.Get(It.IsAny<int>())).Returns(GetSingleTournament(1));
        var controller = new TournamentController(_mockService.Object);
        OkObjectResult okResult = null;

        // act
        var result = controller.GetById(-1);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.Get(It.IsAny<int>()), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => Assert.IsType<Tournament>(okResult.Value)
        );
    }

    [Fact]
    public void GetById_ReturnsNotFound_ServiceGetCalledOnce()
    {
        // arrange
        _mockService.Setup(service => service.Get(It.IsAny<int>())).Returns((Tournament)null);
        var controller = new TournamentController(_mockService.Object);

        // act
        var result = controller.GetById(-1);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.Get(It.IsAny<int>()), Times.Once),
            () => Assert.IsType<NotFoundResult>(result)
        );
    }

    [Fact]
    public void CreateValidInstance_ReturnsOkWithEntity_ServiceInsertCalledOnce()
    {
        // arrange
        var newInstance = GetSingleTournament(1);

        _mockService.Setup(service => service.Insert(It.IsAny<Tournament>())).Callback(() => {});
        var controller = new TournamentController(_mockService.Object);
        OkObjectResult okResult = null;

        // act
        var result = controller.Create(newInstance);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.Insert(It.IsAny<Tournament>()), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => Assert.IsType<Tournament>(okResult.Value)
        );
    }

    [Fact]
    public void CreateNoInstance_ReturnsBadRequest_ServiceInsertCalledNever()
    {
        // arrange
        var controller = new TournamentController(_mockService.Object);
        BadRequestObjectResult badRequestResult = null;
        string content = null;

        // act
        var result = controller.Create(null);

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
    public void UpdateValidInstance_ReturnsOkWithEntity_ServiceUpdateCalledOnce()
    {
        // arrange
        _mockService.Setup(service => service.Update(It.IsAny<int>(), It.IsAny<Tournament>())).Returns(GetSingleTournament(1));
        var controller = new TournamentController(_mockService.Object);
        OkObjectResult okResult = null;

        // act
        var result = controller.Update(-1, GetSingleTournament(1));

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.Update(It.IsAny<int>(), It.IsAny<Tournament>()), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => Assert.IsType<Tournament>(okResult.Value)
        );
    }

    [Fact]
    public void UpdateNoInstance_ReturnsBadRequest_ServiceUpdateCalledNever()
    {
        // arrange
        var controller = new TournamentController(_mockService.Object);
        BadRequestObjectResult badRequestResult = null;
        string content = null;
        
        // act
        var result = controller.Update(-1, null);

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
    public void Delete_ReturnsOk_ServiceDeleteCalledOnce()
    {
        // arrange
        _mockService.Setup(service => service.Delete(It.IsAny<int>())).Callback(() => {});
        var controller = new TournamentController(_mockService.Object);

        // act
        var result = controller.Delete(-1);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.Delete(It.IsAny<int>()), Times.Once),
            () => Assert.IsType<OkResult>(result)
        );
    }

    private static Tournament GetSingleTournament(int id)
    {
        return new Tournament
        {
            Id = id * -1,
            Name = $"Test_Tournament_{id}",
            CreatedDate = new DateTime(2024, 1, 1),
            ModifiedDate = new DateTime(2024, 1, 1),
        };
    }

    private static IEnumerable<Tournament> GetListTournament(int count)
    {
        var arr = new Tournament[count];
        for (var i = 0; i < count; i++)
        {
            arr[i] = GetSingleTournament(i + 1);
        }

        return arr;
    }
}