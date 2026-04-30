using Competition.Domain.Interfaces;
using Design.Domain.Entities;
using Design.Domain.Enums;
using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using Generation.Application.Tournaments.Commands;
using Moq;
using Xunit;

namespace Test.Generation.Application.Tournaments;

public class UT_GenerateCompetitionCommandHandler
{
    [Fact]
    public async Task Handle_ValidTournamentWithNoRounds_CreatesCompetition()
    {
        // arrange
        var tournamentId = Guid.NewGuid();
        var tournament = new Tournament(new TournamentId(tournamentId), TournamentName.Create("Test"), Sport.TableTennis);

        var tournamentRepositoryMock = new Mock<ITournamentRepository>();
        var roundRepositoryMock = new Mock<IRoundRepository>();
        var pouleRepositoryMock = new Mock<IPouleRepository>();
        var competitionRepositoryMock = new Mock<ICompetitionRepository>();

        tournamentRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<TournamentId>())).ReturnsAsync(tournament);
        roundRepositoryMock.Setup(r => r.GetAllByTournamentAsync(It.IsAny<TournamentId>())).ReturnsAsync([]);
        competitionRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Competition.Domain.Entities.Competition>())).Returns(Task.CompletedTask);

        var handler = new GenerateCompetitionCommandHandler(
            tournamentRepositoryMock.Object,
            roundRepositoryMock.Object,
            pouleRepositoryMock.Object,
            competitionRepositoryMock.Object);

        // act
        await handler.Handle(new GenerateCompetitionCommand(tournamentId), CancellationToken.None);

        // assert
        competitionRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Competition.Domain.Entities.Competition>()), Times.Once);
    }

    [Fact]
    public async Task Handle_TournamentNotFound_ThrowsArgumentException()
    {
        // arrange
        var tournamentRepositoryMock = new Mock<ITournamentRepository>();
        var roundRepositoryMock = new Mock<IRoundRepository>();
        var pouleRepositoryMock = new Mock<IPouleRepository>();
        var competitionRepositoryMock = new Mock<ICompetitionRepository>();

        tournamentRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<TournamentId>())).ReturnsAsync((Tournament?)null);

        var handler = new GenerateCompetitionCommandHandler(
            tournamentRepositoryMock.Object,
            roundRepositoryMock.Object,
            pouleRepositoryMock.Object,
            competitionRepositoryMock.Object);

        // act & assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            handler.Handle(new GenerateCompetitionCommand(Guid.NewGuid()), CancellationToken.None));
    }
}
