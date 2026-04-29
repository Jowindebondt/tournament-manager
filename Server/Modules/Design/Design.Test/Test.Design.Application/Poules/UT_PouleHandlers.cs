using AutoMapper;
using Design.Application.DTOs;
using Design.Application.Poules.Commands;
using Design.Application.Poules.Queries;
using Design.Domain.Entities;
using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using Moq;
using Xunit;

namespace Test.Design.Application.Poules;

public class UT_PouleHandlers
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IPouleRepository> _pouleRepositoryMock;
    private readonly Mock<IRoundRepository> _roundRepositoryMock;

    public UT_PouleHandlers()
    {
        _mapperMock = new Mock<IMapper>();
        _pouleRepositoryMock = new Mock<IPouleRepository>();
        _roundRepositoryMock = new Mock<IRoundRepository>();
    }

    [Fact]
    public async Task GetAllPoulesByRoundAndTournamentQueryHandler_ReturnsAllPoules()
    {
        // arrange
        var roundId = Guid.NewGuid();
        var tournamentId = Guid.NewGuid();
        var poules = new List<Poule>
        {
            new(new PouleId(Guid.NewGuid()), PouleName.Create("Poule A"), PoulePlayersCount.Create(4), new RoundId(roundId)),
            new(new PouleId(Guid.NewGuid()), PouleName.Create("Poule B"), PoulePlayersCount.Create(4), new RoundId(roundId))
        };
        var pouleDtos = poules.Select(p => new PouleDto { Id = p.Id.Value, Name = p.Name.Value, TotalPlayers = p.TotalPlayers.Value });

        _pouleRepositoryMock.Setup(r => r.GetAllByRoundAndTournamentAsync(It.IsAny<TournamentId>(), It.IsAny<RoundId>())).ReturnsAsync(poules);
        _mapperMock.Setup(m => m.Map<IEnumerable<PouleDto>>(poules)).Returns(pouleDtos);

        var handler = new GetAllPoulesByRoundAndTournamentQueryHandler(_mapperMock.Object, _pouleRepositoryMock.Object);

        // act
        var result = await handler.Handle(new GetAllPoulesByRoundAndTournamentQuery(roundId, tournamentId), CancellationToken.None);

        // assert
        Assert.Multiple(
            () => Assert.Equal(2, result.Count()),
            () => _pouleRepositoryMock.Verify(r => r.GetAllByRoundAndTournamentAsync(It.IsAny<TournamentId>(), It.IsAny<RoundId>()), Times.Once)
        );
    }

    [Fact]
    public async Task GetPouleByIdQueryHandler_ExistingId_ReturnsPouleDto()
    {
        // arrange
        var pouleId = Guid.NewGuid();
        var poule = new Poule(new PouleId(pouleId), PouleName.Create("Poule A"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var dto = new PouleDto { Id = pouleId, Name = "Poule A" };

        _pouleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<PouleId>())).ReturnsAsync(poule);
        _mapperMock.Setup(m => m.Map<PouleDto?>(poule)).Returns(dto);

        var handler = new GetPouleByIdQueryHandler(_mapperMock.Object, _pouleRepositoryMock.Object);

        // act
        var result = await handler.Handle(new GetPouleByIdQuery(pouleId), CancellationToken.None);

        // assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(pouleId, result!.Id)
        );
    }

    [Fact]
    public async Task GetPouleByIdQueryHandler_NonExistingId_ReturnsNull()
    {
        // arrange
        _pouleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<PouleId>())).ReturnsAsync((Poule?)null);
        _mapperMock.Setup(m => m.Map<PouleDto?>(null)).Returns((PouleDto?)null);

        var handler = new GetPouleByIdQueryHandler(_mapperMock.Object, _pouleRepositoryMock.Object);

        // act
        var result = await handler.Handle(new GetPouleByIdQuery(Guid.NewGuid()), CancellationToken.None);

        // assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreatePouleCommandHandler_ValidCommand_CreatesPouleAndReturnsDto()
    {
        // arrange
        var roundId = Guid.NewGuid();
        var round = new Round(new RoundId(roundId), RoundName.Create("Round"), new TournamentId(Guid.NewGuid()));

        Poule? addedPoule = null;
        _roundRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync(round);
        _pouleRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Poule>()))
            .Callback<Poule>(p => addedPoule = p)
            .Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<PouleDto>(It.IsAny<Poule>()))
            .Returns<Poule>(p => new PouleDto { Id = p.Id.Value, Name = p.Name.Value, TotalPlayers = p.TotalPlayers.Value });

        var handler = new CreatePouleCommandHandler(_mapperMock.Object, _pouleRepositoryMock.Object, _roundRepositoryMock.Object);

        // act
        var result = await handler.Handle(new CreatePouleCommand("New Poule", 4, roundId), CancellationToken.None);

        // assert
        Assert.Multiple(
            () => Assert.NotNull(addedPoule),
            () => Assert.Equal("New Poule", result.Name),
            () => Assert.Equal(4, result.TotalPlayers),
            () => _pouleRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Poule>()), Times.Once)
        );
    }

    [Fact]
    public async Task CreatePouleCommandHandler_NonExistingRound_ThrowsArgumentException()
    {
        // arrange
        _roundRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync((Round?)null);

        var handler = new CreatePouleCommandHandler(_mapperMock.Object, _pouleRepositoryMock.Object, _roundRepositoryMock.Object);

        // act & assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(new CreatePouleCommand("Poule", 4, Guid.NewGuid()), CancellationToken.None));
    }

    [Fact]
    public async Task RenamePouleCommandHandler_ExistingId_RenamesPoule()
    {
        // arrange
        var pouleId = Guid.NewGuid();
        var poule = new Poule(new PouleId(pouleId), PouleName.Create("Old Name"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));

        _pouleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<PouleId>())).ReturnsAsync(poule);
        _pouleRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Poule>())).Returns(Task.CompletedTask);

        var handler = new RenamePouleCommandHandler(_pouleRepositoryMock.Object);

        // act
        await handler.Handle(new RenamePouleCommand(pouleId, "New Name"), CancellationToken.None);

        // assert
        Assert.Multiple(
            () => Assert.Equal("New Name", poule.Name.Value),
            () => _pouleRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Poule>()), Times.Once)
        );
    }

    [Fact]
    public async Task RenamePouleCommandHandler_NonExistingId_ThrowsArgumentException()
    {
        // arrange
        _pouleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<PouleId>())).ReturnsAsync((Poule?)null);

        var handler = new RenamePouleCommandHandler(_pouleRepositoryMock.Object);

        // act & assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(new RenamePouleCommand(Guid.NewGuid(), "New Name"), CancellationToken.None));
    }

    [Fact]
    public async Task SetTotalPlayersPouleCommandHandler_ExistingId_UpdatesTotalPlayers()
    {
        // arrange
        var pouleId = Guid.NewGuid();
        var poule = new Poule(new PouleId(pouleId), PouleName.Create("Poule"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));

        _pouleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<PouleId>())).ReturnsAsync(poule);
        _pouleRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Poule>())).Returns(Task.CompletedTask);

        var handler = new SetTotalPlayersPouleCommandHandler(_pouleRepositoryMock.Object);

        // act
        await handler.Handle(new SetTotalPlayersPouleCommand(pouleId, 8), CancellationToken.None);

        // assert
        Assert.Multiple(
            () => Assert.Equal(8, poule.TotalPlayers.Value),
            () => _pouleRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Poule>()), Times.Once)
        );
    }

    [Fact]
    public async Task SetTotalPlayersPouleCommandHandler_NonExistingId_ThrowsArgumentException()
    {
        // arrange
        _pouleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<PouleId>())).ReturnsAsync((Poule?)null);

        var handler = new SetTotalPlayersPouleCommandHandler(_pouleRepositoryMock.Object);

        // act & assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(new SetTotalPlayersPouleCommand(Guid.NewGuid(), 8), CancellationToken.None));
    }

    [Fact]
    public async Task DeletePouleCommandHandler_ExistingId_DeletesPoule()
    {
        // arrange
        var pouleId = Guid.NewGuid();
        var poule = new Poule(new PouleId(pouleId), PouleName.Create("Poule"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));

        _pouleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<PouleId>())).ReturnsAsync(poule);
        _pouleRepositoryMock.Setup(r => r.RemoveAsync(It.IsAny<Poule>())).Returns(Task.CompletedTask);

        var handler = new DeletePouleCommandHandler(_pouleRepositoryMock.Object);

        // act
        await handler.Handle(new DeletePouleCommand(pouleId), CancellationToken.None);

        // assert
        _pouleRepositoryMock.Verify(r => r.RemoveAsync(It.Is<Poule>(p => p.Id.Value == pouleId)), Times.Once);
    }

    [Fact]
    public async Task DeletePouleCommandHandler_NonExistingId_ThrowsArgumentException()
    {
        // arrange
        _pouleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<PouleId>())).ReturnsAsync((Poule?)null);

        var handler = new DeletePouleCommandHandler(_pouleRepositoryMock.Object);

        // act & assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(new DeletePouleCommand(Guid.NewGuid()), CancellationToken.None));
    }
}
