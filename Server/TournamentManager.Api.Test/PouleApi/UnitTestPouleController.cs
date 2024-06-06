using Microsoft.AspNetCore.Mvc;
using Moq;
using TournamentManager.Application;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Api.Test;

public class UnitTestPouleController
{
    private readonly Mock<IPouleService> _mockService;
    private readonly PouleController _controller;

    public UnitTestPouleController()
    {
        _mockService = new Mock<IPouleService>();
        _controller = new PouleController(_mockService.Object);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetList_ReturnsOkWithFilledList_ServiceGetAllCalledOnce()
    {
        // arrange
        _mockService.Setup(service => service.GetAll(It.IsAny<int>())).Returns(PouleBuilder.GetListPoule(5, 1));
        OkObjectResult okResult = null;
        IEnumerable<Poule> content = null;

        // act
        var result = _controller.GetList(-1);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.GetAll(It.IsAny<int>()), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => content = Assert.IsAssignableFrom<IEnumerable<Poule>>(okResult.Value),
            () => Assert.Equal(5, content.Count())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetList_ReturnsOkWithEmptyList_ServiceGetAllCalledOnce()
    {
        // arrange
        _mockService.Setup(service => service.GetAll(It.IsAny<int>())).Returns((IEnumerable<Poule>)null);
        OkObjectResult okResult = null;

        // act
        var result = _controller.GetList(-1);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.GetAll(It.IsAny<int>()), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.Null(okResult.Value)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetById_ReturnsOkWithEntity_ServiceGetCalledOnce()
    {
        // arrange
        _mockService.Setup(service => service.Get(It.IsAny<int>())).Returns(PouleBuilder.GetSinglePoule());
        OkObjectResult okResult = null;

        // act
        var result = _controller.GetById(-1);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.Get(It.IsAny<int>()), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => Assert.IsType<Poule>(okResult.Value)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetById_ReturnsNotFound_ServiceGetCalledOnce()
    {
        // arrange
        _mockService.Setup(service => service.Get(It.IsAny<int>())).Returns((Poule)null);

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
        var newInstance = PouleBuilder.GetSinglePoule();
        OkObjectResult okResult = null;

        // act
        var result = _controller.Create(newInstance);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.Insert(It.IsAny<Poule>()), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => Assert.IsType<Poule>(okResult.Value)
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
            () => _mockService.Verify(service => service.Insert(It.IsAny<Poule>()), Times.Never),
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
        _mockService.Setup(service => service.Update(It.IsAny<int>(), It.IsAny<Poule>())).Returns(PouleBuilder.GetSinglePoule());
        OkObjectResult okResult = null;

        // act
        var result = _controller.Update(-1, PouleBuilder.GetSinglePoule());

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.Update(It.IsAny<int>(), It.IsAny<Poule>()), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => Assert.IsType<Poule>(okResult.Value)
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
            () => _mockService.Verify(service => service.Update(It.IsAny<int>(), It.IsAny<Poule>()), Times.Never),
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
    public void AddMembers_ReturnsOk_ServiceAddMembersCalledOnce()
    {
        // arrange
        _mockService.Setup(service => service.AddMembers(It.IsAny<int>(), It.IsAny<IEnumerable<int>>())).Callback(() => { });

        // act
        var result = _controller.AddMembers(-1, [-1,-2,-3]);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.AddMembers(It.IsAny<int>(), It.IsAny<IEnumerable<int>>()), Times.Once),
            () => Assert.IsType<OkResult>(result)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void AddNoMembers_ReturnsBadRequest_ServiceAddMembersCalledNever()
    {
        // arrange
        _mockService.Setup(service => service.AddMembers(It.IsAny<int>(), It.IsAny<IEnumerable<int>>())).Callback(() => { });

        // act
        var result = _controller.AddMembers(-1, []);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.AddMembers(It.IsAny<int>(), It.IsAny<IEnumerable<int>>()), Times.Never),
            () => Assert.IsType<BadRequestObjectResult>(result)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void AddMembersAsTeam_ReturnsOk_ServiceAddMembersCalledOnce()
    {
        // arrange
        _mockService.Setup(service => service.AddMembersAsTeam(It.IsAny<int>(), It.IsAny<IEnumerable<int>>())).Callback(() => { });

        // act
        var result = _controller.AddMembersAsTeam(-1, [-1,-2,-3]);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.AddMembersAsTeam(It.IsAny<int>(), It.IsAny<IEnumerable<int>>()), Times.Once),
            () => Assert.IsType<OkResult>(result)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void AddNoMembersAsTeam_ReturnsBadRequest_ServiceAddMembersCalledNever()
    {
        // arrange
        _mockService.Setup(service => service.AddMembersAsTeam(It.IsAny<int>(), It.IsAny<IEnumerable<int>>())).Callback(() => { });

        // act
        var result = _controller.AddMembersAsTeam(-1, []);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(service => service.AddMembersAsTeam(It.IsAny<int>(), It.IsAny<IEnumerable<int>>()), Times.Never),
            () => Assert.IsType<BadRequestObjectResult>(result)
        );
    }
}
