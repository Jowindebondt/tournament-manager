using Moq;
using TournamentManager.Domain;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class TableTennisTestService
{
    private readonly TableTennisService _service;
    private readonly Mock<TableTennisRoundGenerator> _mockRoundGenerator;
    private readonly Mock<TableTennisPouleGenerator> _mockPouleGenerator;
    private readonly Mock<TableTennisMatchGenerator> _mockMatchGenerator;
    private readonly Mock<TableTennisGameGenerator> _mockGameGenerator;

    public TableTennisTestService()
    {
        _mockRoundGenerator = new Mock<TableTennisRoundGenerator>(null!);
        _mockPouleGenerator = new Mock<TableTennisPouleGenerator>(null!, null!);
        _mockMatchGenerator = new Mock<TableTennisMatchGenerator>(null!, null!);
        _mockGameGenerator = new Mock<TableTennisGameGenerator>(null!);
        _service = new TableTennisService(_mockRoundGenerator.Object, _mockPouleGenerator.Object, _mockMatchGenerator.Object, _mockGameGenerator.Object);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Generate_RoundGeneratorCalledOnce_OtherCalledNever()
    {
        // Arrange
        var tournament = new Tournament { Name = "Test" };
        Tournament forwardedTournament = null!;
        _mockRoundGenerator.Setup(gen => gen.Generate(It.IsAny<Tournament>())).Callback<Tournament>(tournament => { forwardedTournament = tournament; });

        // Act
        _service.Generate(tournament);

        // Assert
        Assert.Multiple(
            () => _mockRoundGenerator.Verify(gen => gen.Generate(It.IsAny<Tournament>()), Times.Once),
            () => _mockPouleGenerator.Verify(gen => gen.Generate(It.IsAny<Tournament>()), Times.Never),
            () => _mockMatchGenerator.Verify(gen => gen.Generate(It.IsAny<Tournament>()), Times.Never),
            () => _mockGameGenerator.Verify(gen => gen.Generate(It.IsAny<Tournament>()), Times.Never),
            () => Assert.NotNull(forwardedTournament),
            () => Assert.Equal(tournament, forwardedTournament)
        );
    }
}
