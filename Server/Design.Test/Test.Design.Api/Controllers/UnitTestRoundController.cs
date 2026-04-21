using AutoMapper;
using Design.Api.ViewModels;
using Design.Application.DTOs;
using Design.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TournamentManager.TestHelper;
using Xunit;

namespace Test.Design.Api.Controllers;

public class UnitTestRoundController
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IRoundService> _mockService;
    private readonly global::Design.Api.Controllers.RoundController _controller;

    public UnitTestRoundController()
    {
        _mockMapper = new Mock<IMapper>();
        _mockService = new Mock<IRoundService>();
        _controller = new global::Design.Api.Controllers.RoundController(_mockMapper.Object, _mockService.Object);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task GetAllByTournamentAsync_ReturnsOk_WithFilledList_ServiceCalledOnce()
    {
        // arrange
        var tournamentId = Guid.NewGuid();
        var roundDtos = new List<RoundDto> { new RoundDto { Id = Guid.NewGuid(), Name = "R1" }, new RoundDto { Id = Guid.NewGuid(), Name = "R2" } };
        var viewModels = new List<RoundViewModel> { new RoundViewModel { Id = Guid.NewGuid(), Name = "R1" }, new RoundViewModel { Id = Guid.NewGuid(), Name = "R2" } };
        _mockService.Setup(s => s.GetAllByTournamentAsync(tournamentId)).ReturnsAsync(roundDtos);
        _mockMapper.Setup(m => m.Map<IEnumerable<RoundViewModel>>(roundDtos)).Returns(viewModels);
        OkObjectResult okResult = null!;
        IEnumerable<RoundViewModel> content = null!;

        // act
        var result = await _controller.GetAllByTournamentAsync(tournamentId);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.GetAllByTournamentAsync(tournamentId), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => content = Assert.IsAssignableFrom<IEnumerable<RoundViewModel>>(okResult.Value),
            () => Assert.Equal(2, content.Count())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task GetAllByTournamentAsync_ReturnsOk_WithEmptyList_ServiceCalledOnce()
    {
        // arrange
        var tournamentId = Guid.NewGuid();
        var roundDtos = new List<RoundDto>();
        var viewModels = new List<RoundViewModel>();
        _mockService.Setup(s => s.GetAllByTournamentAsync(tournamentId)).ReturnsAsync(roundDtos);
        _mockMapper.Setup(m => m.Map<IEnumerable<RoundViewModel>>(roundDtos)).Returns(viewModels);
        OkObjectResult okResult = null!;
        IEnumerable<RoundViewModel> content = null!;

        // act
        var result = await _controller.GetAllByTournamentAsync(tournamentId);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.GetAllByTournamentAsync(tournamentId), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => content = Assert.IsAssignableFrom<IEnumerable<RoundViewModel>>(okResult.Value),
            () => Assert.Empty(content)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task GetByIdAsync_ReturnsOk_WithEntity_ServiceCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        var dto = new RoundDto { Id = id, Name = "Round1" };
        var viewModel = new RoundViewModel { Id = id, Name = "Round1" };
        _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(dto);
        _mockMapper.Setup(m => m.Map<RoundViewModel>(dto)).Returns(viewModel);
        OkObjectResult okResult = null!;

        // act
        var result = await _controller.GetByIdAsync(id);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.GetByIdAsync(id), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => Assert.IsType<RoundViewModel>(okResult.Value)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task GetByIdAsync_ReturnsNotFound_ServiceCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((RoundDto)null!);

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
        var createViewModel = new CreateRoundViewModel { Name = "New Round", TournamentId = Guid.NewGuid() };
        var createDto = new CreateRoundDto { Name = "New Round" };
        var id = Guid.NewGuid();
        var createdDto = new RoundDto { Id = id, Name = "New Round" };
        var viewModel = new RoundViewModel { Id = id, Name = "New Round" };
        _mockMapper.Setup(m => m.Map<CreateRoundDto>(createViewModel)).Returns(createDto);
        _mockService.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(createdDto);
        _mockMapper.Setup(m => m.Map<RoundViewModel>(createdDto)).Returns(viewModel);
        CreatedAtActionResult createdResult = null!;

        // act
        var result = await _controller.CreateAsync(createViewModel);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.CreateAsync(createDto), Times.Once),
            () => createdResult = Assert.IsType<CreatedAtActionResult>(result),
            () => Assert.NotNull(createdResult.Value),
            () => Assert.IsType<RoundViewModel>(createdResult.Value)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task RenameAsync_ReturnsNoContent_ServiceCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        var renameViewModel = new RenameRoundViewModel { Name = "Renamed Round" };
        var renameDto = new RenameRoundDto { Id = id, Name = "Renamed Round" };
        _mockMapper.Setup(m => m.Map<RenameRoundDto>(It.IsAny<object>(), It.IsAny<Action<IMappingOperationOptions>>())).Returns(renameDto);
        _mockService.Setup(s => s.RenameAsync(It.IsAny<RenameRoundDto>())).Returns(Task.CompletedTask);

        // act
        var result = await _controller.RenameAsync(id, renameViewModel);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.RenameAsync(It.IsAny<RenameRoundDto>()), Times.Once),
            () => Assert.IsType<NoContentResult>(result)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task SetPreviousRoundAsync_ReturnsNoContent_ServiceCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        var setPreviousViewModel = new SetPreviousRoundViewModel { PreviousId = Guid.NewGuid() };
        var setPreviousDto = new SetPreviousRoundDto { Id = id, PreviousId = setPreviousViewModel.PreviousId };
        _mockMapper.Setup(m => m.Map<SetPreviousRoundDto>(It.IsAny<object>(), It.IsAny<Action<IMappingOperationOptions>>())).Returns(setPreviousDto);
        _mockService.Setup(s => s.SetPreviousRoundAsync(It.IsAny<SetPreviousRoundDto>())).Returns(Task.CompletedTask);

        // act
        var result = await _controller.SetPreviousRoundAsync(id, setPreviousViewModel);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.SetPreviousRoundAsync(It.IsAny<SetPreviousRoundDto>()), Times.Once),
            () => Assert.IsType<NoContentResult>(result)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task SetTableTennisSettingsAsync_ReturnsNoContent_ServiceCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        var setSettingsViewModel = new SetTableTennisSettingsRoundViewModel { BestOf = 5 };
        var setSettingsDto = new SetTableTennisRoundSettingsDto { Id = id, BestOf = 5 };
        _mockMapper.Setup(m => m.Map<SetTableTennisRoundSettingsDto>(It.IsAny<object>(), It.IsAny<Action<IMappingOperationOptions>>())).Returns(setSettingsDto);
        _mockService.Setup(s => s.SetTableTennisSettingsAsync(It.IsAny<SetTableTennisRoundSettingsDto>())).Returns(Task.CompletedTask);

        // act
        var result = await _controller.SetTableTennisSettingsAsync(id, setSettingsViewModel);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.SetTableTennisSettingsAsync(It.IsAny<SetTableTennisRoundSettingsDto>()), Times.Once),
            () => Assert.IsType<NoContentResult>(result)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task SetRoundPoulePositions_ReturnsNoContent_ServiceCalledForEachPosition()
    {
        // arrange
        var id = Guid.NewGuid();
        var positions = new List<SetRoundPoulePositionViewModel>
        {
            new SetRoundPoulePositionViewModel { CurrentPouleId = Guid.NewGuid(), CurrentPosition = 1, PreviousPouleId = Guid.NewGuid(), PreviousPosition = 2 },
            new SetRoundPoulePositionViewModel { CurrentPouleId = Guid.NewGuid(), CurrentPosition = 2, PreviousPouleId = Guid.NewGuid(), PreviousPosition = 3 }
        };
        _mockMapper.Setup(m => m.Map<SetRoundPoulePositionDto>(It.IsAny<SetRoundPoulePositionViewModel>(), It.IsAny<Action<IMappingOperationOptions>>()))
            .Returns(new SetRoundPoulePositionDto());
        _mockService.Setup(s => s.SetRoundPoulePositionAsync(It.IsAny<SetRoundPoulePositionDto>())).Returns(Task.CompletedTask);

        // act
        var result = await _controller.SetRoundPoulePositions(id, positions);

        // assert
        Assert.Multiple(
            () => _mockService.Verify(s => s.SetRoundPoulePositionAsync(It.IsAny<SetRoundPoulePositionDto>()), Times.Exactly(positions.Count)),
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
