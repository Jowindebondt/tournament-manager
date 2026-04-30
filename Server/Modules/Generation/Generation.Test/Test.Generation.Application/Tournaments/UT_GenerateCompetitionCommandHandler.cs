using Competition.Application.Interfaces;
using Design.Application.DTOs;
using Design.Application.Interfaces;
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
        var tournament = new TournamentDto { Id = tournamentId, Name = "Test", Sport = Design.Domain.Enums.Sport.TableTennis };

        var designModuleMock = new Mock<IDesignModuleApi>();
        var competitionModuleMock = new Mock<ICompetitionModuleApi>();

        designModuleMock.Setup(d => d.GetTournamentAsync(tournamentId)).ReturnsAsync(tournament);
        designModuleMock.Setup(d => d.GetRoundsByTournamentAsync(tournamentId)).ReturnsAsync([]);
        competitionModuleMock.Setup(c => c.CreateCompetitionAsync(It.IsAny<CompetitionCreationDto>())).Returns(Task.CompletedTask);

        var handler = new GenerateCompetitionCommandHandler(
            designModuleMock.Object,
            competitionModuleMock.Object);

        // act
        await handler.Handle(new GenerateCompetitionCommand(tournamentId), CancellationToken.None);

        // assert
        competitionModuleMock.Verify(c => c.CreateCompetitionAsync(It.IsAny<CompetitionCreationDto>()), Times.Once);
    }

    [Fact]
    public async Task Handle_TournamentNotFound_ThrowsArgumentException()
    {
        // arrange
        var designModuleMock = new Mock<IDesignModuleApi>();
        var competitionModuleMock = new Mock<ICompetitionModuleApi>();

        designModuleMock.Setup(d => d.GetTournamentAsync(It.IsAny<Guid>())).ReturnsAsync((TournamentDto?)null);

        var handler = new GenerateCompetitionCommandHandler(
            designModuleMock.Object,
            competitionModuleMock.Object);

        // act & assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            handler.Handle(new GenerateCompetitionCommand(Guid.NewGuid()), CancellationToken.None));
    }
}
