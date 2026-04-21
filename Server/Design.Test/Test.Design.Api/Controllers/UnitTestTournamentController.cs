using AutoMapper;
using Design.Api.ViewModels;
using Design.Application.DTOs;
using Design.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TournamentManager.TestHelper;
using Xunit;

namespace Test.Design.Api.Controllers;

public class UnitTestTournamentController
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ITournamentService> _mockService;
    private readonly global::Design.Api.Controllers.TournamentController _controller;

    public UnitTestTournamentController()
    {
        _mockMapper = new Mock<IMapper>();
        _mockService = new Mock<ITournamentService>();
        _controller = new global::Design.Api.Controllers.TournamentController(_mockMapper.Object, _mockService.Object);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task GetAllAsync_ReturnsOk_WithFilledList_ServiceCalledOnce()
    {
        // arrange
        var tournamentDtos = new List<TournamentDTO> { new TournamentDTO { Id = Guid.NewGuid(), Name = "T1" }, new TournamentDTO { Id = Guid.NewGuid(), Name = "T2" } };
        var viewModels = new List<TournamentViewModel> { new TournamentViewModel { Id = Guid.NewGuid(), Name = "T1" }, new TournamentViewModel { Id = Guid.NewGuid(), Name = "T2" } };
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(tournamentDtos);
        _mockMapper.Setup(m => m.Map<IEnumerable<TournamentViewModel>>(tournamentDtos)).Returns(viewModels);
        OkObjectResult okResult = null!;
        IEnumerable<TournamentViewModel> content = null!;

        // act
        var result = await _controller.GetAllAsync();

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.GetAllAsync(), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => content = Assert.IsAssignableFrom<IEnumerable<TournamentViewModel>>(okResult.Value),
            () => Assert.Equal(2, content.Count())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task GetAllAsync_ReturnsOk_WithEmptyList_ServiceCalledOnce()
    {
        // arrange
        var tournamentDtos = new List<TournamentDTO>();
        var viewModels = new List<TournamentViewModel>();
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(tournamentDtos);
        _mockMapper.Setup(m => m.Map<IEnumerable<TournamentViewModel>>(tournamentDtos)).Returns(viewModels);
        OkObjectResult okResult = null!;
        IEnumerable<TournamentViewModel> content = null!;

        // act
        var result = await _controller.GetAllAsync();

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.GetAllAsync(), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => content = Assert.IsAssignableFrom<IEnumerable<TournamentViewModel>>(okResult.Value),
            () => Assert.Empty(content)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task GetByIdAsync_ReturnsOk_WithEntity_ServiceCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        var dto = new TournamentDTO { Id = id, Name = "Tournament1" };
        var viewModel = new TournamentViewModel { Id = id, Name = "Tournament1" };
        _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(dto);
        _mockMapper.Setup(m => m.Map<TournamentViewModel>(dto)).Returns(viewModel);
        OkObjectResult okResult = null!;

        // act
        var result = await _controller.GetByIdAsync(id);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.GetByIdAsync(id), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => Assert.IsType<TournamentViewModel>(okResult.Value)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task GetByIdAsync_ReturnsNotFound_ServiceCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((TournamentDTO)null!);

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
        var createViewModel = new CreateTournamentViewModel { Name = "New Tournament", Sport = "TableTennis" };
        var createDto = new CreateTournamentDTO { Name = "New Tournament" };
        var id = Guid.NewGuid();
        var createdDto = new TournamentDTO { Id = id, Name = "New Tournament" };
        var viewModel = new TournamentViewModel { Id = id, Name = "New Tournament" };
        _mockMapper.Setup(m => m.Map<CreateTournamentDTO>(createViewModel)).Returns(createDto);
        _mockService.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(createdDto);
        _mockMapper.Setup(m => m.Map<TournamentViewModel>(createdDto)).Returns(viewModel);
        CreatedAtActionResult createdResult = null!;

        // act
        var result = await _controller.CreateAsync(createViewModel);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.CreateAsync(createDto), Times.Once),
            () => createdResult = Assert.IsType<CreatedAtActionResult>(result),
            () => Assert.NotNull(createdResult.Value),
            () => Assert.IsType<TournamentViewModel>(createdResult.Value)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task RenameAsync_ReturnsNoContent_ServiceCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        var renameViewModel = new RenameTournamentViewModel { Name = "Renamed" };
        var renameDto = new RenameTournamentDTO { Id = id, Name = "Renamed" };
        _mockMapper.Setup(m => m.Map<RenameTournamentDTO>(It.IsAny<object>(), It.IsAny<Action<IMappingOperationOptions>>())).Returns(renameDto);
        _mockService.Setup(s => s.RenameAsync(It.IsAny<RenameTournamentDTO>())).Returns(Task.CompletedTask);

        // act
        var result = await _controller.RenameAsync(id, renameViewModel);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.RenameAsync(It.IsAny<RenameTournamentDTO>()), Times.Once),
            () => Assert.IsType<NoContentResult>(result)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task LoadTemplateAsync_ReturnsNoContent_ServiceCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        var templateId = Guid.NewGuid();
        _mockService.Setup(s => s.LoadTemplateAsync(id, templateId)).Returns(Task.CompletedTask);

        // act
        var result = await _controller.LoadTemplateAsync(id, templateId);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.LoadTemplateAsync(id, templateId), Times.Once),
            () => Assert.IsType<NoContentResult>(result)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task GenerateAsync_ReturnsNoContent_ServiceCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        _mockService.Setup(s => s.GenerateAsync(id)).Returns(Task.CompletedTask);

        // act
        var result = await _controller.GenerateAsync(id);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.GenerateAsync(id), Times.Once),
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
