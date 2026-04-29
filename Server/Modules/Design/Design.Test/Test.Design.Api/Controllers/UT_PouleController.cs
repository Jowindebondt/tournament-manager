using AutoMapper;
using Design.Api.ViewModels;
using Design.Application.DTOs;
using Design.Application.Poules.Commands;
using Design.Application.Poules.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Test.Design.Api.Controllers;

public class UT_PouleController
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly global::Design.Api.Controllers.PouleController _controller;

    public UT_PouleController()
    {
        _mapperMock = new Mock<IMapper>();
        _mediatorMock = new Mock<IMediator>();
        _controller = new global::Design.Api.Controllers.PouleController(_mapperMock.Object, _mediatorMock.Object);
    }

    [Fact]
    public async Task GetAllByTournamentAndRoundAsync_ReturnsOk_WithFilledList_MediatorCalledOnce()
    {
        // arrange
        var tournamentId = Guid.NewGuid();
        var roundId = Guid.NewGuid();
        var pouleDtos = new List<PouleDto>
        {
            new PouleDto { Id = Guid.NewGuid(), Name = "P1" },
            new PouleDto { Id = Guid.NewGuid(), Name = "P2" }
        };
        var viewModels = new List<PouleViewModel>
        {
            new PouleViewModel { Id = Guid.NewGuid(), Name = "P1" },
            new PouleViewModel { Id = Guid.NewGuid(), Name = "P2" }
        };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllPoulesByRoundAndTournamentQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(pouleDtos);
        _mapperMock.Setup(m => m.Map<IEnumerable<PouleViewModel>>(pouleDtos)).Returns(viewModels);
        OkObjectResult okResult = null!;
        IEnumerable<PouleViewModel> content = null!;

        // act
        var result = await _controller.GetAllByTournamentAndRoundAsync(tournamentId, roundId);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllPoulesByRoundAndTournamentQuery>(), It.IsAny<CancellationToken>()), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => content = Assert.IsAssignableFrom<IEnumerable<PouleViewModel>>(okResult.Value),
            () => Assert.Equal(2, content.Count())
        );
    }

    [Fact]
    public async Task GetAllByTournamentAndRoundAsync_ReturnsOk_WithEmptyList_MediatorCalledOnce()
    {
        // arrange
        var tournamentId = Guid.NewGuid();
        var roundId = Guid.NewGuid();
        var pouleDtos = new List<PouleDto>();
        var viewModels = new List<PouleViewModel>();
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllPoulesByRoundAndTournamentQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(pouleDtos);
        _mapperMock.Setup(m => m.Map<IEnumerable<PouleViewModel>>(pouleDtos)).Returns(viewModels);
        OkObjectResult okResult = null!;
        IEnumerable<PouleViewModel> content = null!;

        // act
        var result = await _controller.GetAllByTournamentAndRoundAsync(tournamentId, roundId);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllPoulesByRoundAndTournamentQuery>(), It.IsAny<CancellationToken>()), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => content = Assert.IsAssignableFrom<IEnumerable<PouleViewModel>>(okResult.Value),
            () => Assert.Empty(content)
        );
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsOk_MediatorCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        var dto = new PouleDto { Id = id, Name = "Poule1" };
        var viewModel = new PouleViewModel { Id = id, Name = "Poule1" };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetPouleByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(dto);
        _mapperMock.Setup(m => m.Map<PouleViewModel>(dto)).Returns(viewModel);
        OkObjectResult okResult = null!;

        // act
        var result = await _controller.GetByIdAsync(id);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<GetPouleByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => Assert.IsType<PouleViewModel>(okResult.Value)
        );
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNotFound_MediatorCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetPouleByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((PouleDto?)null);

        // act
        var result = await _controller.GetByIdAsync(id);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<GetPouleByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once),
            () => Assert.IsType<NotFoundResult>(result)
        );
    }

    [Fact]
    public async Task CreateAsync_ValidInput_ReturnsCreatedAtAction_MediatorCalledOnce()
    {
        // arrange
        var createViewModel = new CreatePouleViewModel { Name = "New Poule", TotalPlayers = 4, RoundId = Guid.NewGuid() };
        var id = Guid.NewGuid();
        var createdDto = new PouleDto { Id = id, Name = "New Poule", TotalPlayers = 4 };
        var viewModel = new PouleViewModel { Id = id, Name = "New Poule", TotalPlayers = 4 };
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreatePouleCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(createdDto);
        _mapperMock.Setup(m => m.Map<PouleViewModel>(createdDto)).Returns(viewModel);
        CreatedAtActionResult createdResult = null!;

        // act
        var result = await _controller.CreateAsync(createViewModel);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<CreatePouleCommand>(), It.IsAny<CancellationToken>()), Times.Once),
            () => createdResult = Assert.IsType<CreatedAtActionResult>(result),
            () => Assert.NotNull(createdResult.Value),
            () => Assert.IsType<PouleViewModel>(createdResult.Value)
        );
    }

    [Fact]
    public async Task RenameAsync_ReturnsNoContent_MediatorCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        var renameViewModel = new RenamePouleViewModel { Name = "Renamed Poule" };
        _mediatorMock.Setup(m => m.Send(It.IsAny<RenamePouleCommand>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // act
        var result = await _controller.RenameAsync(id, renameViewModel);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<RenamePouleCommand>(), It.IsAny<CancellationToken>()), Times.Once),
            () => Assert.IsType<NoContentResult>(result)
        );
    }

    [Fact]
    public async Task SetTotalPlayersAsync_ReturnsNoContent_MediatorCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        var setTotalPlayersViewModel = new SetTotalPlayersPouleViewModel { TotalPlayers = 6 };
        _mediatorMock.Setup(m => m.Send(It.IsAny<SetTotalPlayersPouleCommand>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // act
        var result = await _controller.SetTotalPlayersAsync(id, setTotalPlayersViewModel);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<SetTotalPlayersPouleCommand>(), It.IsAny<CancellationToken>()), Times.Once),
            () => Assert.IsType<NoContentResult>(result)
        );
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNoContent_MediatorCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeletePouleCommand>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // act
        var result = await _controller.DeleteAsync(id);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<DeletePouleCommand>(), It.IsAny<CancellationToken>()), Times.Once),
            () => Assert.IsType<NoContentResult>(result)
        );
    }
}
