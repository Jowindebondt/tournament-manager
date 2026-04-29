using AutoMapper;
using Design.Api.ViewModels;
using Design.Application.DTOs;
using Design.Application.Rounds.Commands;
using Design.Application.Rounds.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Test.Design.Api.Controllers;

public class UT_RoundController
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly global::Design.Api.Controllers.RoundController _controller;

    public UT_RoundController()
    {
        _mapperMock = new Mock<IMapper>();
        _mediatorMock = new Mock<IMediator>();
        _controller = new global::Design.Api.Controllers.RoundController(_mapperMock.Object, _mediatorMock.Object);
    }

    [Fact]
    public async Task GetAllByTournamentAsync_ReturnsOk_WithFilledList_MediatorCalledOnce()
    {
        // arrange
        var tournamentId = Guid.NewGuid();
        var roundDtos = new List<RoundDto>
        {
            new RoundDto { Id = Guid.NewGuid(), Name = "R1" },
            new RoundDto { Id = Guid.NewGuid(), Name = "R2" }
        };
        var viewModels = new List<RoundViewModel>
        {
            new RoundViewModel { Id = Guid.NewGuid(), Name = "R1" },
            new RoundViewModel { Id = Guid.NewGuid(), Name = "R2" }
        };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllRoundsByTournamentQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(roundDtos);
        _mapperMock.Setup(m => m.Map<IEnumerable<RoundViewModel>>(roundDtos)).Returns(viewModels);
        OkObjectResult okResult = null!;
        IEnumerable<RoundViewModel> content = null!;

        // act
        var result = await _controller.GetAllByTournamentAsync(tournamentId);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllRoundsByTournamentQuery>(), It.IsAny<CancellationToken>()), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => content = Assert.IsAssignableFrom<IEnumerable<RoundViewModel>>(okResult.Value),
            () => Assert.Equal(2, content.Count())
        );
    }

    [Fact]
    public async Task GetAllByTournamentAsync_ReturnsOk_WithEmptyList_MediatorCalledOnce()
    {
        // arrange
        var tournamentId = Guid.NewGuid();
        var roundDtos = new List<RoundDto>();
        var viewModels = new List<RoundViewModel>();
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllRoundsByTournamentQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(roundDtos);
        _mapperMock.Setup(m => m.Map<IEnumerable<RoundViewModel>>(roundDtos)).Returns(viewModels);
        OkObjectResult okResult = null!;
        IEnumerable<RoundViewModel> content = null!;

        // act
        var result = await _controller.GetAllByTournamentAsync(tournamentId);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllRoundsByTournamentQuery>(), It.IsAny<CancellationToken>()), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => content = Assert.IsAssignableFrom<IEnumerable<RoundViewModel>>(okResult.Value),
            () => Assert.Empty(content)
        );
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsOk_MediatorCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        var dto = new RoundDto { Id = id, Name = "Round1" };
        var viewModel = new RoundViewModel { Id = id, Name = "Round1" };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetRoundByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(dto);
        _mapperMock.Setup(m => m.Map<RoundViewModel>(dto)).Returns(viewModel);
        OkObjectResult okResult = null!;

        // act
        var result = await _controller.GetByIdAsync(id);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<GetRoundByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => Assert.IsType<RoundViewModel>(okResult.Value)
        );
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNotFound_MediatorCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetRoundByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((RoundDto?)null);

        // act
        var result = await _controller.GetByIdAsync(id);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<GetRoundByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once),
            () => Assert.IsType<NotFoundResult>(result)
        );
    }

    [Fact]
    public async Task CreateAsync_ValidInput_ReturnsCreatedAtAction_MediatorCalledOnce()
    {
        // arrange
        var createViewModel = new CreateRoundViewModel { Name = "New Round", TournamentId = Guid.NewGuid() };
        var id = Guid.NewGuid();
        var createdDto = new RoundDto { Id = id, Name = "New Round" };
        var viewModel = new RoundViewModel { Id = id, Name = "New Round" };
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateRoundCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(createdDto);
        _mapperMock.Setup(m => m.Map<RoundViewModel>(createdDto)).Returns(viewModel);
        CreatedAtActionResult createdResult = null!;

        // act
        var result = await _controller.CreateAsync(createViewModel);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<CreateRoundCommand>(), It.IsAny<CancellationToken>()), Times.Once),
            () => createdResult = Assert.IsType<CreatedAtActionResult>(result),
            () => Assert.NotNull(createdResult.Value),
            () => Assert.IsType<RoundViewModel>(createdResult.Value)
        );
    }

    [Fact]
    public async Task RenameAsync_ReturnsNoContent_MediatorCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        var renameViewModel = new RenameRoundViewModel { Name = "Renamed Round" };
        _mediatorMock.Setup(m => m.Send(It.IsAny<RenameRoundCommand>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // act
        var result = await _controller.RenameAsync(id, renameViewModel);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<RenameRoundCommand>(), It.IsAny<CancellationToken>()), Times.Once),
            () => Assert.IsType<NoContentResult>(result)
        );
    }

    [Fact]
    public async Task SetPreviousRoundAsync_ReturnsNoContent_MediatorCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        var setPreviousViewModel = new SetPreviousRoundViewModel { PreviousId = Guid.NewGuid() };
        _mediatorMock.Setup(m => m.Send(It.IsAny<SetPreviousRoundCommand>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // act
        var result = await _controller.SetPreviousRoundAsync(id, setPreviousViewModel);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<SetPreviousRoundCommand>(), It.IsAny<CancellationToken>()), Times.Once),
            () => Assert.IsType<NoContentResult>(result)
        );
    }

    [Fact]
    public async Task SetTableTennisSettingsAsync_ValidBestOf_ReturnsNoContent_MediatorCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        var setSettingsViewModel = new SetTableTennisSettingsRoundViewModel { BestOf = 5 };
        _mediatorMock.Setup(m => m.Send(It.IsAny<SetRoundSettingsCommand>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // act
        var result = await _controller.SetTableTennisSettingsAsync(id, setSettingsViewModel);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<SetRoundSettingsCommand>(), It.IsAny<CancellationToken>()), Times.Once),
            () => Assert.IsType<NoContentResult>(result)
        );
    }

    [Fact]
    public async Task SetRoundPoulePositions_ReturnsNoContent_MediatorCalledForEachPosition()
    {
        // arrange
        var id = Guid.NewGuid();
        var positions = new List<SetRoundPoulePositionViewModel>
        {
            new SetRoundPoulePositionViewModel { CurrentPouleId = Guid.NewGuid(), CurrentPosition = 1, PreviousPouleId = Guid.NewGuid(), PreviousPosition = 1 },
            new SetRoundPoulePositionViewModel { CurrentPouleId = Guid.NewGuid(), CurrentPosition = 2, PreviousPouleId = Guid.NewGuid(), PreviousPosition = 2 }
        };
        _mediatorMock.Setup(m => m.Send(It.IsAny<SetRoundPoulePositionCommand>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // act
        var result = await _controller.SetRoundPoulePositions(id, positions);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<SetRoundPoulePositionCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(positions.Count)),
            () => Assert.IsType<NoContentResult>(result)
        );
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNoContent_MediatorCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteRoundCommand>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // act
        var result = await _controller.DeleteAsync(id);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteRoundCommand>(), It.IsAny<CancellationToken>()), Times.Once),
            () => Assert.IsType<NoContentResult>(result)
        );
    }
}
