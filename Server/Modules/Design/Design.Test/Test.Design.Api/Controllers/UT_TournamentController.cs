using AutoMapper;
using Design.Api.ViewModels;
using Design.Application.DTOs;
using Design.Application.Tournaments.Commands;
using Design.Application.Tournaments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Test.Design.Api.Controllers;

public class UT_TournamentController
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly global::Design.Api.Controllers.TournamentController _controller;

    public UT_TournamentController()
    {
        _mapperMock = new Mock<IMapper>();
        _mediatorMock = new Mock<IMediator>();
        _controller = new global::Design.Api.Controllers.TournamentController(_mapperMock.Object, _mediatorMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOk_WithFilledList_MediatorCalledOnce()
    {
        // arrange
        var tournamentDtos = new List<TournamentDto>
        {
            new TournamentDto { Id = Guid.NewGuid(), Name = "T1" },
            new TournamentDto { Id = Guid.NewGuid(), Name = "T2" }
        };
        var viewModels = new List<TournamentViewModel>
        {
            new TournamentViewModel { Id = Guid.NewGuid(), Name = "T1" },
            new TournamentViewModel { Id = Guid.NewGuid(), Name = "T2" }
        };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllTournamentsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(tournamentDtos);
        _mapperMock.Setup(m => m.Map<IEnumerable<TournamentViewModel>>(tournamentDtos)).Returns(viewModels);
        OkObjectResult okResult = null!;
        IEnumerable<TournamentViewModel> content = null!;

        // act
        var result = await _controller.GetAllAsync();

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllTournamentsQuery>(), It.IsAny<CancellationToken>()), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => content = Assert.IsAssignableFrom<IEnumerable<TournamentViewModel>>(okResult.Value),
            () => Assert.Equal(2, content.Count())
        );
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOk_WithEmptyList_MediatorCalledOnce()
    {
        // arrange
        var tournamentDtos = new List<TournamentDto>();
        var viewModels = new List<TournamentViewModel>();
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllTournamentsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(tournamentDtos);
        _mapperMock.Setup(m => m.Map<IEnumerable<TournamentViewModel>>(tournamentDtos)).Returns(viewModels);
        OkObjectResult okResult = null!;
        IEnumerable<TournamentViewModel> content = null!;

        // act
        var result = await _controller.GetAllAsync();

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllTournamentsQuery>(), It.IsAny<CancellationToken>()), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => content = Assert.IsAssignableFrom<IEnumerable<TournamentViewModel>>(okResult.Value),
            () => Assert.Empty(content)
        );
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsOk_MediatorCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        var dto = new TournamentDto { Id = id, Name = "Tournament1" };
        var viewModel = new TournamentViewModel { Id = id, Name = "Tournament1" };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetTournamentByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(dto);
        _mapperMock.Setup(m => m.Map<TournamentViewModel>(dto)).Returns(viewModel);
        OkObjectResult okResult = null!;

        // act
        var result = await _controller.GetByIdAsync(id);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<GetTournamentByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once),
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => Assert.IsType<TournamentViewModel>(okResult.Value)
        );
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNotFound_MediatorCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetTournamentByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((TournamentDto?)null);

        // act
        var result = await _controller.GetByIdAsync(id);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<GetTournamentByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once),
            () => Assert.IsType<NotFoundResult>(result)
        );
    }

    [Fact]
    public async Task CreateAsync_ValidSport_ReturnsCreatedAtAction_MediatorCalledOnce()
    {
        // arrange
        var createViewModel = new CreateTournamentViewModel { Name = "New Tournament", Sport = "TableTennis" };
        var id = Guid.NewGuid();
        var createdDto = new TournamentDto { Id = id, Name = "New Tournament" };
        var viewModel = new TournamentViewModel { Id = id, Name = "New Tournament" };
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateTournamentCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(createdDto);
        _mapperMock.Setup(m => m.Map<TournamentViewModel>(createdDto)).Returns(viewModel);
        CreatedAtActionResult createdResult = null!;

        // act
        var result = await _controller.CreateAsync(createViewModel);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<CreateTournamentCommand>(), It.IsAny<CancellationToken>()), Times.Once),
            () => createdResult = Assert.IsType<CreatedAtActionResult>(result),
            () => Assert.NotNull(createdResult.Value),
            () => Assert.IsType<TournamentViewModel>(createdResult.Value)
        );
    }

    [Fact]
    public async Task CreateAsync_InvalidSport_ReturnsBadRequest()
    {
        // arrange
        var createViewModel = new CreateTournamentViewModel { Name = "New Tournament", Sport = "InvalidSport" };

        // act
        var result = await _controller.CreateAsync(createViewModel);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<CreateTournamentCommand>(), It.IsAny<CancellationToken>()), Times.Never),
            () => Assert.IsType<BadRequestObjectResult>(result)
        );
    }

    [Fact]
    public async Task RenameAsync_ReturnsNoContent_MediatorCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        var renameViewModel = new RenameTournamentViewModel { Name = "Renamed" };
        _mediatorMock.Setup(m => m.Send(It.IsAny<RenameTournamentCommand>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // act
        var result = await _controller.RenameAsync(id, renameViewModel);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<RenameTournamentCommand>(), It.IsAny<CancellationToken>()), Times.Once),
            () => Assert.IsType<NoContentResult>(result)
        );
    }

    [Fact]
    public async Task GenerateAsync_ReturnsNoContent_MediatorCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(It.IsAny<GenerateTournamentCommand>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // act
        var result = await _controller.GenerateAsync(id);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<GenerateTournamentCommand>(), It.IsAny<CancellationToken>()), Times.Once),
            () => Assert.IsType<NoContentResult>(result)
        );
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNoContent_MediatorCalledOnce()
    {
        // arrange
        var id = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteTournamentCommand>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // act
        var result = await _controller.DeleteAsync(id);

        // assert
        Assert.Multiple(
            () => _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteTournamentCommand>(), It.IsAny<CancellationToken>()), Times.Once),
            () => Assert.IsType<NoContentResult>(result)
        );
    }
}
