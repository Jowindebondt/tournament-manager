using AutoMapper;
using Design.Application.DTOs;
using Design.Application.Services;
using Design.Domain;
using Design.Domain.Entities;
using Design.Domain.ValueObjects;
using Moq;
using TournamentManager.TestHelper;
using Xunit;

namespace Test.Design.Application.Services;

public class UT_PouleService
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IPouleRepository> _mockPouleRepository;
    private readonly PouleService _service;

    public UT_PouleService()
    {
        _mockMapper = new Mock<IMapper>();
        _mockPouleRepository = new Mock<IPouleRepository>();
        _service = new PouleService(_mockMapper.Object, _mockPouleRepository.Object, null!);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task GetAllByRoundAndTournamentAsync_RepoGetAllCalledOnce_ReturnsMappedDTOs()
    {
        // Arrange
        var roundId = Guid.NewGuid();
        var tournamentId = Guid.NewGuid();
        var poules = new List<Poule>();
        var pouleDtos = new List<PouleDto>();
        _mockPouleRepository.Setup(r => r.GetAllByTournamentAndRoundAsync(It.IsAny<TournamentId>(), It.IsAny<RoundId>())).ReturnsAsync(poules);
        _mockMapper.Setup(m => m.Map<IEnumerable<PouleDto>>(poules)).Returns(pouleDtos);

        // Act
        var result = await _service.GetAllByRoundAndTournamentAsync(roundId, tournamentId);

        // Assert
        _mockPouleRepository.Verify(r => r.GetAllByTournamentAndRoundAsync(It.IsAny<TournamentId>(), It.IsAny<RoundId>()), Times.Once);
        Assert.Equal(pouleDtos, result);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task GetByIdAsync_RepoGetByIdCalledOnce_ReturnsMappedDTO()
    {
        // Arrange
        var id = Guid.NewGuid();
        var poule = new Poule(new PouleId(id), PouleName.Create("TestPoule"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var pouleDto = new PouleDto { Id = id, Name = "TestPoule" };
        _mockPouleRepository.Setup(r => r.GetByIdAsync(It.IsAny<PouleId>())).ReturnsAsync(poule);
        _mockMapper.Setup(m => m.Map<PouleDto>(poule)).Returns(pouleDto);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        _mockPouleRepository.Verify(r => r.GetByIdAsync(It.IsAny<PouleId>()), Times.Once);
        Assert.Equal(pouleDto, result);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task CreateAsync_RoundServiceGetByIdAndRepoAddCalledOnce_ReturnsMappedDTO()
    {
        // Arrange
        var roundId = Guid.NewGuid();
        var round = new Round(new RoundId(roundId), RoundName.Create("Round1"), new TournamentId(Guid.NewGuid()));
        var roundDto = new RoundDto { Id = roundId, Name = "Round1" };
        var pouleDto = new PouleDto { Name = "TestPoule" };

        var mockRoundRepository = new Mock<IRoundRepository>();
        mockRoundRepository.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync(round);
        _mockMapper.Setup(m => m.Map<RoundDto>(round)).Returns(roundDto);
        _mockMapper.Setup(m => m.Map<PouleDto>(It.IsAny<Poule>())).Returns(pouleDto);
        _mockPouleRepository.Setup(r => r.AddAsync(It.IsAny<Poule>())).Returns(Task.CompletedTask);

        var roundService = new RoundService(_mockMapper.Object, mockRoundRepository.Object, null!, null!);
        var service = new PouleService(_mockMapper.Object, _mockPouleRepository.Object, roundService);

        var createDto = new CreatePouleDto { Name = "TestPoule", TotalPlayers = 4, RoundId = roundId };

        // Act
        var result = await service.CreateAsync(createDto);

        // Assert
        mockRoundRepository.Verify(r => r.GetByIdAsync(It.IsAny<RoundId>()), Times.Once);
        Assert.Multiple(
            () => _mockPouleRepository.Verify(r => r.AddAsync(It.IsAny<Poule>()), Times.Once),
            () => Assert.Equal(pouleDto, result)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task RenameAsync_ExistingPoule_RepoGetByIdAndUpdateCalledOnce()
    {
        // Arrange
        var id = Guid.NewGuid();
        var poule = new Poule(new PouleId(id), PouleName.Create("OldName"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var renameDto = new RenamePouleDto { Id = id, Name = "NewName" };
        _mockPouleRepository.Setup(r => r.GetByIdAsync(It.IsAny<PouleId>())).ReturnsAsync(poule);
        _mockPouleRepository.Setup(r => r.UpdateAsync(It.IsAny<Poule>())).Returns(Task.CompletedTask);

        // Act
        await _service.RenameAsync(renameDto);

        // Assert
        Assert.Multiple(
            () => _mockPouleRepository.Verify(r => r.GetByIdAsync(It.IsAny<PouleId>()), Times.Once),
            () => _mockPouleRepository.Verify(r => r.UpdateAsync(It.IsAny<Poule>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task RenameAsync_NonExistingPoule_ThrowsArgumentException()
    {
        // Arrange
        var renameDto = new RenamePouleDto { Id = Guid.NewGuid(), Name = "NewName" };
        _mockPouleRepository.Setup(r => r.GetByIdAsync(It.IsAny<PouleId>())).ReturnsAsync((Poule)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.RenameAsync(renameDto));
        Assert.Multiple(
            () => _mockPouleRepository.Verify(r => r.GetByIdAsync(It.IsAny<PouleId>()), Times.Once),
            () => _mockPouleRepository.Verify(r => r.UpdateAsync(It.IsAny<Poule>()), Times.Never)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task SetTotalPlayersAsync_ExistingPoule_RepoGetByIdAndUpdateCalledOnce()
    {
        // Arrange
        var id = Guid.NewGuid();
        var poule = new Poule(new PouleId(id), PouleName.Create("TestPoule"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        var setDto = new SetTotalPlayersPouleDto { Id = id, TotalPlayers = 6 };
        _mockPouleRepository.Setup(r => r.GetByIdAsync(It.IsAny<PouleId>())).ReturnsAsync(poule);
        _mockPouleRepository.Setup(r => r.UpdateAsync(It.IsAny<Poule>())).Returns(Task.CompletedTask);

        // Act
        await _service.SetTotalPlayersAsync(setDto);

        // Assert
        Assert.Multiple(
            () => _mockPouleRepository.Verify(r => r.GetByIdAsync(It.IsAny<PouleId>()), Times.Once),
            () => _mockPouleRepository.Verify(r => r.UpdateAsync(It.IsAny<Poule>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task SetTotalPlayersAsync_NonExistingPoule_ThrowsArgumentException()
    {
        // Arrange
        var setDto = new SetTotalPlayersPouleDto { Id = Guid.NewGuid(), TotalPlayers = 6 };
        _mockPouleRepository.Setup(r => r.GetByIdAsync(It.IsAny<PouleId>())).ReturnsAsync((Poule)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.SetTotalPlayersAsync(setDto));
        Assert.Multiple(
            () => _mockPouleRepository.Verify(r => r.GetByIdAsync(It.IsAny<PouleId>()), Times.Once),
            () => _mockPouleRepository.Verify(r => r.UpdateAsync(It.IsAny<Poule>()), Times.Never)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task DeleteAsync_ExistingPoule_RepoGetByIdAndRemoveCalledOnce()
    {
        // Arrange
        var id = Guid.NewGuid();
        var poule = new Poule(new PouleId(id), PouleName.Create("TestPoule"), PoulePlayersCount.Create(4), new RoundId(Guid.NewGuid()));
        _mockPouleRepository.Setup(r => r.GetByIdAsync(It.IsAny<PouleId>())).ReturnsAsync(poule);
        _mockPouleRepository.Setup(r => r.RemoveAsync(It.IsAny<Poule>())).Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(id);

        // Assert
        Assert.Multiple(
            () => _mockPouleRepository.Verify(r => r.GetByIdAsync(It.IsAny<PouleId>()), Times.Once),
            () => _mockPouleRepository.Verify(r => r.RemoveAsync(It.IsAny<Poule>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task DeleteAsync_NonExistingPoule_ThrowsArgumentException()
    {
        // Arrange
        _mockPouleRepository.Setup(r => r.GetByIdAsync(It.IsAny<PouleId>())).ReturnsAsync((Poule)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.DeleteAsync(Guid.NewGuid()));
        Assert.Multiple(
            () => _mockPouleRepository.Verify(r => r.GetByIdAsync(It.IsAny<PouleId>()), Times.Once),
            () => _mockPouleRepository.Verify(r => r.RemoveAsync(It.IsAny<Poule>()), Times.Never)
        );
    }
}
