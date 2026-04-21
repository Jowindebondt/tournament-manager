using AutoMapper;
using Design.Application.DTOs;
using Design.Application.Services;
using Design.Domain;
using Design.Domain.Entities;
using Design.Domain.Enums;
using Design.Domain.ValueObjects;
using Moq;
using TournamentManager.TestHelper;
using Xunit;

namespace Test.Design.Application.Services;

public class UT_TournamentService
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ITournamentRepository> _mockTournamentRepository;
    private readonly TournamentService _service;

    public UT_TournamentService()
    {
        _mockMapper = new Mock<IMapper>();
        _mockTournamentRepository = new Mock<ITournamentRepository>();
        _service = new TournamentService(_mockMapper.Object, _mockTournamentRepository.Object);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task GetAllAsync_RepoGetAllCalledOnce_ReturnsMappedDTOs()
    {
        // Arrange
        var tournaments = new List<Tournament>();
        var tournamentDtos = new List<TournamentDTO>();
        _mockTournamentRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(tournaments);
        _mockMapper.Setup(m => m.Map<IEnumerable<TournamentDTO>>(tournaments)).Returns(tournamentDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        _mockTournamentRepository.Verify(r => r.GetAllAsync(), Times.Once);
        Assert.Equal(tournamentDtos, result);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task GetByIdAsync_RepoGetByIdCalledOnce_ReturnsMappedDTO()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tournament = new Tournament(new TournamentId(id), TournamentName.Create("Test"), Sport.TableTennis);
        var tournamentDto = new TournamentDTO { Id = id, Name = "Test" };
        _mockTournamentRepository.Setup(r => r.GetByIdAsync(It.IsAny<TournamentId>())).ReturnsAsync(tournament);
        _mockMapper.Setup(m => m.Map<TournamentDTO>(tournament)).Returns(tournamentDto);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        _mockTournamentRepository.Verify(r => r.GetByIdAsync(It.IsAny<TournamentId>()), Times.Once);
        Assert.Equal(tournamentDto, result);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task CreateAsync_RepoAddCalledOnce_ReturnsMappedDTO()
    {
        // Arrange
        var createDto = new CreateTournamentDTO { Name = "Test", Sport = Sport.TableTennis };
        var tournamentDto = new TournamentDTO { Name = "Test" };
        _mockTournamentRepository.Setup(r => r.AddAsync(It.IsAny<Tournament>())).Returns(Task.CompletedTask);
        _mockMapper.Setup(m => m.Map<TournamentDTO>(It.IsAny<Tournament>())).Returns(tournamentDto);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        _mockTournamentRepository.Verify(r => r.AddAsync(It.IsAny<Tournament>()), Times.Once);
        Assert.Equal(tournamentDto, result);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task RenameAsync_ExistingTournament_RepoGetByIdAndUpdateCalledOnce()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tournament = new Tournament(new TournamentId(id), TournamentName.Create("OldName"), Sport.TableTennis);
        var renameDto = new RenameTournamentDTO { Id = id, Name = "NewName" };
        _mockTournamentRepository.Setup(r => r.GetByIdAsync(It.IsAny<TournamentId>())).ReturnsAsync(tournament);
        _mockTournamentRepository.Setup(r => r.UpdateAsync(It.IsAny<Tournament>())).Returns(Task.CompletedTask);

        // Act
        await _service.RenameAsync(renameDto);

        // Assert
        Assert.Multiple(
            () => _mockTournamentRepository.Verify(r => r.GetByIdAsync(It.IsAny<TournamentId>()), Times.Once),
            () => _mockTournamentRepository.Verify(r => r.UpdateAsync(It.IsAny<Tournament>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task RenameAsync_NonExistingTournament_ThrowsArgumentException()
    {
        // Arrange
        var renameDto = new RenameTournamentDTO { Id = Guid.NewGuid(), Name = "NewName" };
        _mockTournamentRepository.Setup(r => r.GetByIdAsync(It.IsAny<TournamentId>())).ReturnsAsync((Tournament?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.RenameAsync(renameDto));
        Assert.Multiple(
            () => _mockTournamentRepository.Verify(r => r.GetByIdAsync(It.IsAny<TournamentId>()), Times.Once),
            () => _mockTournamentRepository.Verify(r => r.UpdateAsync(It.IsAny<Tournament>()), Times.Never)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task DeleteAsync_ExistingTournament_RepoGetByIdAndRemoveCalledOnce()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tournament = new Tournament(new TournamentId(id), TournamentName.Create("Test"), Sport.TableTennis);
        _mockTournamentRepository.Setup(r => r.GetByIdAsync(It.IsAny<TournamentId>())).ReturnsAsync(tournament);
        _mockTournamentRepository.Setup(r => r.RemoveAsync(It.IsAny<Tournament>())).Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(id);

        // Assert
        Assert.Multiple(
            () => _mockTournamentRepository.Verify(r => r.GetByIdAsync(It.IsAny<TournamentId>()), Times.Once),
            () => _mockTournamentRepository.Verify(r => r.RemoveAsync(It.IsAny<Tournament>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task DeleteAsync_NonExistingTournament_ThrowsArgumentException()
    {
        // Arrange
        _mockTournamentRepository.Setup(r => r.GetByIdAsync(It.IsAny<TournamentId>())).ReturnsAsync((Tournament?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.DeleteAsync(Guid.NewGuid()));
        Assert.Multiple(
            () => _mockTournamentRepository.Verify(r => r.GetByIdAsync(It.IsAny<TournamentId>()), Times.Once),
            () => _mockTournamentRepository.Verify(r => r.RemoveAsync(It.IsAny<Tournament>()), Times.Never)
        );
    }
}
