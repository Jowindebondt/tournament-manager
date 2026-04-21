using AutoMapper;
using Design.Api.ViewModels;
using Design.Application.DTOs;
using Design.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TournamentManager.TestHelper;
using Xunit;

namespace Test.Design.Api.Controllers;

public class UnitTestPouleController
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IPouleService> _mockService;
    private readonly global::Design.Api.Controllers.PouleController _controller;

    public UnitTestPouleController()
    {
        _mockMapper = new Mock<IMapper>();
        _mockService = new Mock<IPouleService>();
        _controller = new global::Design.Api.Controllers.PouleController(_mockMapper.Object, _mockService.Object);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task GetAllByTournamentAndRoundAsync_ReturnsOk_WithFilledList_ServiceCalledOnce()
    {
        // arrange
        var tournamentId = Guid.NewGuid();
        var roundId = Guid.NewGuid();
        var pouleDtos = new List<PouleDto> { new PouleDto { Id = Guid.NewGuid(), Name = "P1" }, new PouleDto { Id = Guid.NewGuid(), Name = "P2" } };
        var viewModels = new List<PouleViewModel> { new PouleViewModel { Id = Guid.NewGuid(), Name = "P1" }, new PouleViewModel { Id = Guid.NewGuid(), Name = "P2" } };
        _mockService.Setup(s => s.GetAllByRoundAndTournamentAsync(roundId, tournamentId)).ReturnsAsync(pouleDtos);
        _mockMapper.Setup(m => m.Map<IEnumerable<PouleViewModel>>(pouleDtos)).Returns(viewModels);
        OkObjectResult okResult = null!;
        IEnumerable<PouleViewModel> content = null!;

        // act
        var result = await _controller.GetAllByTournamentAndRoundAsync(tournamentId, roundId);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.GetAllByRoundAndTournamentAsync(roundId, tournamentId), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => content = Assert.IsAssignableFrom<IEnumerable<PouleViewModel>>(okResult.Value),
            () => Assert.Equal(2, content.Count())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task GetAllByTournamentAndRoundAsync_ReturnsOk_WithEmptyList_ServiceCalledOnce()
    {
        // arrange
        var tournamentId = Guid.NewGuid();
        var roundId = Guid.NewGuid();
        var pouleDtos = new List<PouleDto>();
        var viewModels = new List<PouleViewModel>();
        _mockService.Setup(s => s.GetAllByRoundAndTournamentAsync(roundId, tournamentId)).ReturnsAsync(pouleDtos);
        _mockMapper.Setup(m => m.Map<IEnumerable<PouleViewModel>>(pouleDtos)).Returns(viewModels);
        OkObjectResult okResult = null!;
        IEnumerable<PouleViewModel> content = null!;

        // act
        var result = await _controller.GetAllByTournamentAndRoundAsync(tournamentId, roundId);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.GetAllByRoundAndTournamentAsync(roundId, tournamentId), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => content = Assert.IsAssignableFrom<IEnumerable<PouleViewModel>>(okResult.Value),
            () => Assert.Empty(content)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task GetByIdAsync_ReturnsOk_WithEntity_ServiceCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        var dto = new PouleDto { Id = id, Name = "Poule1" };
        var viewModel = new PouleViewModel { Id = id, Name = "Poule1" };
        _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(dto);
        _mockMapper.Setup(m => m.Map<PouleViewModel>(dto)).Returns(viewModel);
        OkObjectResult okResult = null!;

        // act
        var result = await _controller.GetByIdAsync(id);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.GetByIdAsync(id), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => Assert.IsType<PouleViewModel>(okResult.Value)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task GetByIdAsync_ReturnsNotFound_ServiceCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((PouleDto)null!);

        // act
        var result = await _controller.GetByIdAsync(id);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.GetByIdAsync(id), Times.Once),
            () => Assert.IsType<NotFoundResult>(result)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task CreateAsync_ReturnsCreatedAtAction_WithEntity_ServiceCalledOnce()
    {
        // arrange
        var createViewModel = new CreatePouleViewModel { Name = "New Poule", TotalPlayers = 4, RoundId = Guid.NewGuid() };
        var createDto = new CreatePouleDto { Name = "New Poule", TotalPlayers = 4 };
        var id = Guid.NewGuid();
        var createdDto = new PouleDto { Id = id, Name = "New Poule", TotalPlayers = 4 };
        var viewModel = new PouleViewModel { Id = id, Name = "New Poule", TotalPlayers = 4 };
        _mockMapper.Setup(m => m.Map<CreatePouleDto>(createViewModel)).Returns(createDto);
        _mockService.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(createdDto);
        _mockMapper.Setup(m => m.Map<PouleViewModel>(createdDto)).Returns(viewModel);
        CreatedAtActionResult createdResult = null!;

        // act
        var result = await _controller.CreateAsync(createViewModel);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.CreateAsync(createDto), Times.Once),
            () => createdResult = Assert.IsType<CreatedAtActionResult>(result),
            () => Assert.NotNull(createdResult.Value),
            () => Assert.IsType<PouleViewModel>(createdResult.Value)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task RenameAsync_ReturnsNoContent_ServiceCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        var renameViewModel = new RenamePouleViewModel { Name = "Renamed Poule" };
        var renameDto = new RenamePouleDto { Id = id, Name = "Renamed Poule" };
        _mockMapper.Setup(m => m.Map<RenamePouleDto>(It.IsAny<object>(), It.IsAny<Action<IMappingOperationOptions>>())).Returns(renameDto);
        _mockService.Setup(s => s.RenameAsync(It.IsAny<RenamePouleDto>())).Returns(Task.CompletedTask);

        // act
        var result = await _controller.RenameAsync(id, renameViewModel);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.RenameAsync(It.IsAny<RenamePouleDto>()), Times.Once),
            () => Assert.IsType<NoContentResult>(result)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task SetTotalPlayersAsync_ReturnsNoContent_ServiceCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        var setTotalPlayersViewModel = new SetTotalPlayersPouleViewModel { TotalPlayers = 6 };
        var setTotalPlayersDto = new SetTotalPlayersPouleDto { Id = id, TotalPlayers = 6 };
        _mockMapper.Setup(m => m.Map<SetTotalPlayersPouleDto>(It.IsAny<object>(), It.IsAny<Action<IMappingOperationOptions>>())).Returns(setTotalPlayersDto);
        _mockService.Setup(s => s.SetTotalPlayersAsync(It.IsAny<SetTotalPlayersPouleDto>())).Returns(Task.CompletedTask);

        // act
        var result = await _controller.SetTotalPlayersAsync(id, setTotalPlayersViewModel);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.SetTotalPlayersAsync(It.IsAny<SetTotalPlayersPouleDto>()), Times.Once),
            () => Assert.IsType<NoContentResult>(result)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task DeleteAsync_ReturnsNoContent_ServiceCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        _mockService.Setup(s => s.DeleteAsync(id)).Returns(Task.CompletedTask);

        // act
        var result = await _controller.DeleteAsync(id);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.DeleteAsync(id), Times.Once),
            () => Assert.IsType<NoContentResult>(result)
        );
    }
}
