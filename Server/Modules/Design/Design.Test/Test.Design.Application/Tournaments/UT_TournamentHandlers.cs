using AutoMapper;
using Competition.Domain.Interfaces;
using Design.Application.DTOs;
using Design.Application.Tournaments.Commands;
using Design.Application.Tournaments.Queries;
using Design.Domain.Entities;
using Design.Domain.Enums;
using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using Moq;
using Xunit;

namespace Test.Design.Application.Tournaments;

public class UT_TournamentHandlers
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ITournamentRepository> _tournamentRepositoryMock;
    private readonly Mock<IRoundRepository> _roundRepositoryMock;
    private readonly Mock<IPouleRepository> _pouleRepositoryMock;
    private readonly Mock<ICompetitionRepository> _competitionRepositoryMock;

    public UT_TournamentHandlers()
    {
        _mapperMock = new Mock<IMapper>();
        _tournamentRepositoryMock = new Mock<ITournamentRepository>();
        _roundRepositoryMock = new Mock<IRoundRepository>();
        _pouleRepositoryMock = new Mock<IPouleRepository>();
        _competitionRepositoryMock = new Mock<ICompetitionRepository>();
    }

    [Fact]
    public async Task GetAllTournamentsQueryHandler_ReturnsAllTournaments()
    {
        // arrange
        var tournaments = new List<Tournament>
        {
            new(new TournamentId(Guid.NewGuid()), TournamentName.Create("Tournament A"), Sport.TableTennis),
            new(new TournamentId(Guid.NewGuid()), TournamentName.Create("Tournament B"), Sport.TableTennis)
        };
        var tournamentDtos = tournaments.Select(t => new TournamentDto { Id = t.Id.Value, Name = t.Name.Value, Sport = t.Sport });

        _tournamentRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tournaments);
        _mapperMock.Setup(m => m.Map<IEnumerable<TournamentDto>>(tournaments)).Returns(tournamentDtos);

        var handler = new GetAllTournamentsQueryHandler(_mapperMock.Object, _tournamentRepositoryMock.Object);

        // act
        var result = await handler.Handle(new GetAllTournamentsQuery(), CancellationToken.None);

        // assert
        Assert.Multiple(
            () => Assert.Equal(2, result.Count()),
            () => _tournamentRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once)
        );
    }

    [Fact]
    public async Task GetTournamentByIdQueryHandler_ExistingId_ReturnsTournamentDto()
    {
        // arrange
        var tournamentId = Guid.NewGuid();
        var tournament = new Tournament(new TournamentId(tournamentId), TournamentName.Create("Tournament A"), Sport.TableTennis);
        var dto = new TournamentDto { Id = tournamentId, Name = "Tournament A", Sport = Sport.TableTennis };

        _tournamentRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<TournamentId>())).ReturnsAsync(tournament);
        _mapperMock.Setup(m => m.Map<TournamentDto?>(tournament)).Returns(dto);

        var handler = new GetTournamentByIdQueryHandler(_mapperMock.Object, _tournamentRepositoryMock.Object);

        // act
        var result = await handler.Handle(new GetTournamentByIdQuery(tournamentId), CancellationToken.None);

        // assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(tournamentId, result!.Id),
            () => _tournamentRepositoryMock.Verify(r => r.GetByIdAsync(It.Is<TournamentId>(tid => tid.Value == tournamentId)), Times.Once)
        );
    }

    [Fact]
    public async Task GetTournamentByIdQueryHandler_NonExistingId_ReturnsNull()
    {
        // arrange
        var tournamentId = Guid.NewGuid();

        _tournamentRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<TournamentId>())).ReturnsAsync((Tournament?)null);
        _mapperMock.Setup(m => m.Map<TournamentDto?>(null)).Returns((TournamentDto?)null);

        var handler = new GetTournamentByIdQueryHandler(_mapperMock.Object, _tournamentRepositoryMock.Object);

        // act
        var result = await handler.Handle(new GetTournamentByIdQuery(tournamentId), CancellationToken.None);

        // assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateTournamentCommandHandler_ValidCommand_CreatesTournamentAndReturnsDto()
    {
        // arrange
        Tournament? addedTournament = null;
        _tournamentRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Tournament>()))
            .Callback<Tournament>(t => addedTournament = t)
            .Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<TournamentDto>(It.IsAny<Tournament>()))
            .Returns<Tournament>(t => new TournamentDto { Id = t.Id.Value, Name = t.Name.Value, Sport = t.Sport });

        var handler = new CreateTournamentCommandHandler(_mapperMock.Object, _tournamentRepositoryMock.Object);

        // act
        var result = await handler.Handle(new CreateTournamentCommand("New Tournament", Sport.TableTennis), CancellationToken.None);

        // assert
        Assert.Multiple(
            () => Assert.NotNull(addedTournament),
            () => Assert.Equal("New Tournament", result.Name),
            () => Assert.Equal(Sport.TableTennis, result.Sport),
            () => _tournamentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Tournament>()), Times.Once)
        );
    }

    [Fact]
    public async Task RenameTournamentCommandHandler_ExistingId_RenamesTournament()
    {
        // arrange
        var tournamentId = Guid.NewGuid();
        var tournament = new Tournament(new TournamentId(tournamentId), TournamentName.Create("Old Name"), Sport.TableTennis);

        _tournamentRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<TournamentId>())).ReturnsAsync(tournament);
        _tournamentRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Tournament>())).Returns(Task.CompletedTask);

        var handler = new RenameTournamentCommandHandler(_tournamentRepositoryMock.Object);

        // act
        await handler.Handle(new RenameTournamentCommand(tournamentId, "New Name"), CancellationToken.None);

        // assert
        Assert.Multiple(
            () => Assert.Equal("New Name", tournament.Name.Value),
            () => _tournamentRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Tournament>()), Times.Once)
        );
    }

    [Fact]
    public async Task RenameTournamentCommandHandler_NonExistingId_ThrowsArgumentException()
    {
        // arrange
        _tournamentRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<TournamentId>())).ReturnsAsync((Tournament?)null);

        var handler = new RenameTournamentCommandHandler(_tournamentRepositoryMock.Object);

        // act & assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(new RenameTournamentCommand(Guid.NewGuid(), "New Name"), CancellationToken.None));
    }

    [Fact]
    public async Task DeleteTournamentCommandHandler_ExistingId_DeletesTournament()
    {
        // arrange
        var tournamentId = Guid.NewGuid();
        var tournament = new Tournament(new TournamentId(tournamentId), TournamentName.Create("Tournament"), Sport.TableTennis);

        _tournamentRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<TournamentId>())).ReturnsAsync(tournament);
        _tournamentRepositoryMock.Setup(r => r.RemoveAsync(It.IsAny<Tournament>())).Returns(Task.CompletedTask);

        var handler = new DeleteTournamentCommandHandler(_tournamentRepositoryMock.Object);

        // act
        await handler.Handle(new DeleteTournamentCommand(tournamentId), CancellationToken.None);

        // assert
        _tournamentRepositoryMock.Verify(r => r.RemoveAsync(It.Is<Tournament>(t => t.Id.Value == tournamentId)), Times.Once);
    }

    [Fact]
    public async Task DeleteTournamentCommandHandler_NonExistingId_ThrowsArgumentException()
    {
        // arrange
        _tournamentRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<TournamentId>())).ReturnsAsync((Tournament?)null);

        var handler = new DeleteTournamentCommandHandler(_tournamentRepositoryMock.Object);

        // act & assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(new DeleteTournamentCommand(Guid.NewGuid()), CancellationToken.None));
    }

    [Fact]
    public async Task GenerateTournamentCommandHandler_ExistingTournament_CreatesCompetition()
    {
        // arrange
        var tournamentId = Guid.NewGuid();
        var tournament = new Tournament(new TournamentId(tournamentId), TournamentName.Create("Tournament A"), Sport.TableTennis);
        _tournamentRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<TournamentId>())).ReturnsAsync(tournament);
        _roundRepositoryMock.Setup(r => r.GetAllByTournamentAsync(It.IsAny<TournamentId>())).ReturnsAsync([]);
        _competitionRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Competition.Domain.Entities.Competition>())).Returns(Task.CompletedTask);

        var handler = new GenerateTournamentCommandHandler(
            _tournamentRepositoryMock.Object,
            _roundRepositoryMock.Object,
            _pouleRepositoryMock.Object,
            _competitionRepositoryMock.Object);

        // act
        await handler.Handle(new GenerateTournamentCommand(tournamentId), CancellationToken.None);

        // assert
        _competitionRepositoryMock.Verify(r => r.AddAsync(It.Is<Competition.Domain.Entities.Competition>(
            c => c.Name.Value == "Tournament A")), Times.Once);
    }

    [Fact]
    public async Task GenerateTournamentCommandHandler_NonExistingTournament_ThrowsArgumentException()
    {
        // arrange
        _tournamentRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<TournamentId>())).ReturnsAsync((Tournament?)null);

        var handler = new GenerateTournamentCommandHandler(
            _tournamentRepositoryMock.Object,
            _roundRepositoryMock.Object,
            _pouleRepositoryMock.Object,
            _competitionRepositoryMock.Object);

        // act & assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            handler.Handle(new GenerateTournamentCommand(Guid.NewGuid()), CancellationToken.None));
    }
}
