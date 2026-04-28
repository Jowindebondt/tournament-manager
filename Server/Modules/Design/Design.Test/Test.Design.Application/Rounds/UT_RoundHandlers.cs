using AutoMapper;
using Design.Application.DTOs;
using Design.Application.Rounds.Commands;
using Design.Application.Rounds.Queries;
using Design.Domain.Entities;
using Design.Domain.Enums;
using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using Moq;
using Xunit;

namespace Test.Design.Application.Rounds;

public class UT_RoundHandlers
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRoundRepository> _roundRepositoryMock;
    private readonly Mock<ITournamentRepository> _tournamentRepositoryMock;
    private readonly Mock<IPouleRepository> _pouleRepositoryMock;

    public UT_RoundHandlers()
    {
        _mapperMock = new Mock<IMapper>();
        _roundRepositoryMock = new Mock<IRoundRepository>();
        _tournamentRepositoryMock = new Mock<ITournamentRepository>();
        _pouleRepositoryMock = new Mock<IPouleRepository>();
    }

    [Fact]
    public async Task GetAllRoundsByTournamentQueryHandler_ReturnsAllRounds()
    {
        // arrange
        var tournamentId = Guid.NewGuid();
        var rounds = new List<Round>
        {
            new(new RoundId(Guid.NewGuid()), RoundName.Create("Round A"), new TournamentId(tournamentId)),
            new(new RoundId(Guid.NewGuid()), RoundName.Create("Round B"), new TournamentId(tournamentId))
        };
        var roundDtos = rounds.Select(r => new RoundDto { Id = r.Id.Value, Name = r.Name.Value });

        _roundRepositoryMock.Setup(r => r.GetAllByTournamentAsync(It.IsAny<TournamentId>())).ReturnsAsync(rounds);
        _mapperMock.Setup(m => m.Map<IEnumerable<RoundDto>>(rounds)).Returns(roundDtos);

        var handler = new GetAllRoundsByTournamentQueryHandler(_mapperMock.Object, _roundRepositoryMock.Object);

        // act
        var result = await handler.Handle(new GetAllRoundsByTournamentQuery(tournamentId), CancellationToken.None);

        // assert
        Assert.Multiple(
            () => Assert.Equal(2, result.Count()),
            () => _roundRepositoryMock.Verify(r => r.GetAllByTournamentAsync(It.Is<TournamentId>(tid => tid.Value == tournamentId)), Times.Once)
        );
    }

    [Fact]
    public async Task GetRoundByIdQueryHandler_ExistingId_ReturnsRoundDto()
    {
        // arrange
        var roundId = Guid.NewGuid();
        var round = new Round(new RoundId(roundId), RoundName.Create("Round A"), new TournamentId(Guid.NewGuid()));
        var dto = new RoundDto { Id = roundId, Name = "Round A" };

        _roundRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync(round);
        _mapperMock.Setup(m => m.Map<RoundDto?>(round)).Returns(dto);

        var handler = new GetRoundByIdQueryHandler(_mapperMock.Object, _roundRepositoryMock.Object);

        // act
        var result = await handler.Handle(new GetRoundByIdQuery(roundId), CancellationToken.None);

        // assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(roundId, result!.Id)
        );
    }

    [Fact]
    public async Task GetRoundByIdQueryHandler_NonExistingId_ReturnsNull()
    {
        // arrange
        _roundRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync((Round?)null);
        _mapperMock.Setup(m => m.Map<RoundDto?>(null)).Returns((RoundDto?)null);

        var handler = new GetRoundByIdQueryHandler(_mapperMock.Object, _roundRepositoryMock.Object);

        // act
        var result = await handler.Handle(new GetRoundByIdQuery(Guid.NewGuid()), CancellationToken.None);

        // assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateRoundCommandHandler_ValidCommand_CreatesRoundAndReturnsDto()
    {
        // arrange
        var tournamentId = Guid.NewGuid();
        var tournament = new Tournament(
            new TournamentId(tournamentId),
            TournamentName.Create("Tournament"),
            Sport.TableTennis);

        Round? addedRound = null;
        _tournamentRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<TournamentId>())).ReturnsAsync(tournament);
        _roundRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Round>()))
            .Callback<Round>(r => addedRound = r)
            .Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<RoundDto>(It.IsAny<Round>()))
            .Returns<Round>(r => new RoundDto { Id = r.Id.Value, Name = r.Name.Value });

        var handler = new CreateRoundCommandHandler(_mapperMock.Object, _roundRepositoryMock.Object, _tournamentRepositoryMock.Object);

        // act
        var result = await handler.Handle(new CreateRoundCommand("New Round", tournamentId), CancellationToken.None);

        // assert
        Assert.Multiple(
            () => Assert.NotNull(addedRound),
            () => Assert.Equal("New Round", result.Name),
            () => _roundRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Round>()), Times.Once)
        );
    }

    [Fact]
    public async Task CreateRoundCommandHandler_NonExistingTournament_ThrowsArgumentException()
    {
        // arrange
        _tournamentRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<TournamentId>())).ReturnsAsync((Tournament?)null);

        var handler = new CreateRoundCommandHandler(_mapperMock.Object, _roundRepositoryMock.Object, _tournamentRepositoryMock.Object);

        // act & assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(new CreateRoundCommand("Round", Guid.NewGuid()), CancellationToken.None));
    }

    [Fact]
    public async Task RenameRoundCommandHandler_ExistingId_RenamesRound()
    {
        // arrange
        var roundId = Guid.NewGuid();
        var round = new Round(new RoundId(roundId), RoundName.Create("Old Name"), new TournamentId(Guid.NewGuid()));

        _roundRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync(round);
        _roundRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Round>())).Returns(Task.CompletedTask);

        var handler = new RenameRoundCommandHandler(_roundRepositoryMock.Object);

        // act
        await handler.Handle(new RenameRoundCommand(roundId, "New Name"), CancellationToken.None);

        // assert
        Assert.Multiple(
            () => Assert.Equal("New Name", round.Name.Value),
            () => _roundRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Round>()), Times.Once)
        );
    }

    [Fact]
    public async Task RenameRoundCommandHandler_NonExistingId_ThrowsArgumentException()
    {
        // arrange
        _roundRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync((Round?)null);

        var handler = new RenameRoundCommandHandler(_roundRepositoryMock.Object);

        // act & assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(new RenameRoundCommand(Guid.NewGuid(), "New Name"), CancellationToken.None));
    }

    [Fact]
    public async Task DeleteRoundCommandHandler_ExistingId_DeletesRound()
    {
        // arrange
        var roundId = Guid.NewGuid();
        var round = new Round(new RoundId(roundId), RoundName.Create("Round"), new TournamentId(Guid.NewGuid()));

        _roundRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync(round);
        _roundRepositoryMock.Setup(r => r.RemoveAsync(It.IsAny<Round>())).Returns(Task.CompletedTask);

        var handler = new DeleteRoundCommandHandler(_roundRepositoryMock.Object);

        // act
        await handler.Handle(new DeleteRoundCommand(roundId), CancellationToken.None);

        // assert
        _roundRepositoryMock.Verify(r => r.RemoveAsync(It.Is<Round>(r => r.Id.Value == roundId)), Times.Once);
    }

    [Fact]
    public async Task DeleteRoundCommandHandler_NonExistingId_ThrowsArgumentException()
    {
        // arrange
        _roundRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync((Round?)null);

        var handler = new DeleteRoundCommandHandler(_roundRepositoryMock.Object);

        // act & assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(new DeleteRoundCommand(Guid.NewGuid()), CancellationToken.None));
    }

    [Fact]
    public async Task SetPreviousRoundCommandHandler_ExistingIds_SetsPreviousRound()
    {
        // arrange
        var tournamentId = new TournamentId(Guid.NewGuid());
        var roundId = Guid.NewGuid();
        var previousRoundId = Guid.NewGuid();
        var round = new Round(new RoundId(roundId), RoundName.Create("Round"), tournamentId);
        var previousRound = new Round(new RoundId(previousRoundId), RoundName.Create("Previous Round"), tournamentId);

        _roundRepositoryMock.Setup(r => r.GetByIdAsync(It.Is<RoundId>(rid => rid.Value == roundId))).ReturnsAsync(round);
        _roundRepositoryMock.Setup(r => r.GetByIdAsync(It.Is<RoundId>(rid => rid.Value == previousRoundId))).ReturnsAsync(previousRound);
        _roundRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Round>())).Returns(Task.CompletedTask);

        var handler = new SetPreviousRoundCommandHandler(_roundRepositoryMock.Object);

        // act
        await handler.Handle(new SetPreviousRoundCommand(roundId, previousRoundId), CancellationToken.None);

        // assert
        Assert.Multiple(
            () => Assert.Equal(previousRound, round.PreviousRound),
            () => _roundRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Round>()), Times.Once)
        );
    }

    [Fact]
    public async Task SetPreviousRoundCommandHandler_NonExistingRound_ThrowsArgumentException()
    {
        // arrange
        _roundRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync((Round?)null);

        var handler = new SetPreviousRoundCommandHandler(_roundRepositoryMock.Object);

        // act & assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(new SetPreviousRoundCommand(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None));
    }
}
