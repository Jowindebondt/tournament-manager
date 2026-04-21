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

public class UT_RoundService
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IRoundRepository> _mockRoundRepository;
    private readonly RoundService _service;

    public UT_RoundService()
    {
        _mockMapper = new Mock<IMapper>();
        _mockRoundRepository = new Mock<IRoundRepository>();
        _service = new RoundService(_mockMapper.Object, _mockRoundRepository.Object, null!, null!);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task GetAllByTournamentAsync_RepoGetAllCalledOnce_ReturnsMappedDTOs()
    {
        // Arrange
        var tournamentId = Guid.NewGuid();
        var rounds = new List<Round>();
        var roundDtos = new List<RoundDto>();
        _mockRoundRepository.Setup(r => r.GetAllByTournamentAsync(It.IsAny<TournamentId>())).ReturnsAsync(rounds);
        _mockMapper.Setup(m => m.Map<IEnumerable<RoundDto>>(rounds)).Returns(roundDtos);

        // Act
        var result = await _service.GetAllByTournamentAsync(tournamentId);

        // Assert
        _mockRoundRepository.Verify(r => r.GetAllByTournamentAsync(It.IsAny<TournamentId>()), Times.Once);
        Assert.Equal(roundDtos, result);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task GetByIdAsync_RepoGetByIdCalledOnce_ReturnsMappedDTO()
    {
        // Arrange
        var id = Guid.NewGuid();
        var round = new Round(new RoundId(id), RoundName.Create("Round1"), new TournamentId(Guid.NewGuid()));
        var roundDto = new RoundDto { Id = id, Name = "Round1" };
        _mockRoundRepository.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync(round);
        _mockMapper.Setup(m => m.Map<RoundDto>(round)).Returns(roundDto);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        _mockRoundRepository.Verify(r => r.GetByIdAsync(It.IsAny<RoundId>()), Times.Once);
        Assert.Equal(roundDto, result);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task CreateAsync_TournamentServiceGetByIdAndRepoAddCalledOnce_ReturnsMappedDTO()
    {
        // Arrange
        var tournamentId = Guid.NewGuid();
        var tournament = new Tournament(new TournamentId(tournamentId), TournamentName.Create("TestTournament"), Sport.TableTennis);
        var tournamentDto = new TournamentDTO { Id = tournamentId, Name = "TestTournament" };
        var roundDto = new RoundDto { Name = "Round1" };

        var mockTournamentRepository = new Mock<ITournamentRepository>();
        mockTournamentRepository.Setup(r => r.GetByIdAsync(It.IsAny<TournamentId>())).ReturnsAsync(tournament);
        _mockMapper.Setup(m => m.Map<TournamentDTO>(tournament)).Returns(tournamentDto);
        _mockMapper.Setup(m => m.Map<RoundDto>(It.IsAny<Round>())).Returns(roundDto);
        _mockRoundRepository.Setup(r => r.AddAsync(It.IsAny<Round>())).Returns(Task.CompletedTask);

        var tournamentService = new TournamentService(_mockMapper.Object, mockTournamentRepository.Object);
        var service = new RoundService(_mockMapper.Object, _mockRoundRepository.Object, tournamentService, null!);

        var createDto = new CreateRoundDto { Name = "Round1", TournamentId = tournamentId };

        // Act
        var result = await service.CreateAsync(createDto);

        // Assert
        mockTournamentRepository.Verify(r => r.GetByIdAsync(It.IsAny<TournamentId>()), Times.Once);
        Assert.Multiple(
            () => _mockRoundRepository.Verify(r => r.AddAsync(It.IsAny<Round>()), Times.Once),
            () => Assert.Equal(roundDto, result)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task RenameAsync_ExistingRound_RepoGetByIdAndUpdateCalledOnce()
    {
        // Arrange
        var id = Guid.NewGuid();
        var round = new Round(new RoundId(id), RoundName.Create("OldName"), new TournamentId(Guid.NewGuid()));
        var renameDto = new RenameRoundDto { Id = id, Name = "NewName" };
        _mockRoundRepository.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync(round);
        _mockRoundRepository.Setup(r => r.UpdateAsync(It.IsAny<Round>())).Returns(Task.CompletedTask);

        // Act
        await _service.RenameAsync(renameDto);

        // Assert
        Assert.Multiple(
            () => _mockRoundRepository.Verify(r => r.GetByIdAsync(It.IsAny<RoundId>()), Times.Once),
            () => _mockRoundRepository.Verify(r => r.UpdateAsync(It.IsAny<Round>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task RenameAsync_NonExistingRound_ThrowsArgumentException()
    {
        // Arrange
        var renameDto = new RenameRoundDto { Id = Guid.NewGuid(), Name = "NewName" };
        _mockRoundRepository.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync((Round)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.RenameAsync(renameDto));
        Assert.Multiple(
            () => _mockRoundRepository.Verify(r => r.GetByIdAsync(It.IsAny<RoundId>()), Times.Once),
            () => _mockRoundRepository.Verify(r => r.UpdateAsync(It.IsAny<Round>()), Times.Never)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task SetPreviousRoundAsync_ExistingRounds_RepoUpdateCalledOnce()
    {
        // Arrange
        var id = Guid.NewGuid();
        var previousId = Guid.NewGuid();
        var round = new Round(new RoundId(id), RoundName.Create("Round1"), new TournamentId(Guid.NewGuid()));
        var previousRound = new Round(new RoundId(previousId), RoundName.Create("Round0"), new TournamentId(Guid.NewGuid()));
        var setDto = new SetPreviousRoundDto { Id = id, PreviousId = previousId };

        _mockRoundRepository.SetupSequence(r => r.GetByIdAsync(It.IsAny<RoundId>()))
            .ReturnsAsync(round)
            .ReturnsAsync(previousRound);
        _mockRoundRepository.Setup(r => r.UpdateAsync(It.IsAny<Round>())).Returns(Task.CompletedTask);

        // Act
        await _service.SetPreviousRoundAsync(setDto);

        // Assert
        Assert.Multiple(
            () => _mockRoundRepository.Verify(r => r.GetByIdAsync(It.IsAny<RoundId>()), Times.Exactly(2)),
            () => _mockRoundRepository.Verify(r => r.UpdateAsync(It.IsAny<Round>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task SetPreviousRoundAsync_NonExistingRound_ThrowsArgumentException()
    {
        // Arrange
        var setDto = new SetPreviousRoundDto { Id = Guid.NewGuid(), PreviousId = Guid.NewGuid() };
        _mockRoundRepository.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync((Round)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.SetPreviousRoundAsync(setDto));
        Assert.Multiple(
            () => _mockRoundRepository.Verify(r => r.GetByIdAsync(It.IsAny<RoundId>()), Times.Once),
            () => _mockRoundRepository.Verify(r => r.UpdateAsync(It.IsAny<Round>()), Times.Never)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task SetPreviousRoundAsync_NonExistingPreviousRound_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var round = new Round(new RoundId(id), RoundName.Create("Round1"), new TournamentId(Guid.NewGuid()));
        var setDto = new SetPreviousRoundDto { Id = id, PreviousId = Guid.NewGuid() };

        _mockRoundRepository.SetupSequence(r => r.GetByIdAsync(It.IsAny<RoundId>()))
            .ReturnsAsync(round)
            .ReturnsAsync((Round)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.SetPreviousRoundAsync(setDto));
        Assert.Multiple(
            () => _mockRoundRepository.Verify(r => r.GetByIdAsync(It.IsAny<RoundId>()), Times.Exactly(2)),
            () => _mockRoundRepository.Verify(r => r.UpdateAsync(It.IsAny<Round>()), Times.Never)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task SetTableTennisSettingsAsync_ExistingRound_RepoGetByIdAndUpdateCalledOnce()
    {
        // Arrange
        var id = Guid.NewGuid();
        var round = new Round(new RoundId(id), RoundName.Create("Round1"), new TournamentId(Guid.NewGuid()));
        var setDto = new SetTableTennisRoundSettingsDto { Id = id, BestOf = 3 };
        _mockRoundRepository.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync(round);
        _mockRoundRepository.Setup(r => r.UpdateAsync(It.IsAny<Round>())).Returns(Task.CompletedTask);

        // Act
        await _service.SetTableTennisSettingsAsync(setDto);

        // Assert
        Assert.Multiple(
            () => _mockRoundRepository.Verify(r => r.GetByIdAsync(It.IsAny<RoundId>()), Times.Once),
            () => _mockRoundRepository.Verify(r => r.UpdateAsync(It.IsAny<Round>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task SetTableTennisSettingsAsync_NonExistingRound_ThrowsArgumentException()
    {
        // Arrange
        var setDto = new SetTableTennisRoundSettingsDto { Id = Guid.NewGuid(), BestOf = 3 };
        _mockRoundRepository.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync((Round)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.SetTableTennisSettingsAsync(setDto));
        Assert.Multiple(
            () => _mockRoundRepository.Verify(r => r.GetByIdAsync(It.IsAny<RoundId>()), Times.Once),
            () => _mockRoundRepository.Verify(r => r.UpdateAsync(It.IsAny<Round>()), Times.Never)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task SetRoundPoulePositionAsync_NoPreviousRound_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var round = new Round(new RoundId(id), RoundName.Create("Round1"), new TournamentId(Guid.NewGuid()));
        var setDto = new SetRoundPoulePositionDto { Id = id, CurrentPouleId = Guid.NewGuid(), PreviousPouleId = Guid.NewGuid(), CurrentPosition = 1, PreviousPosition = 1 };
        _mockRoundRepository.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync(round);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.SetRoundPoulePositionAsync(setDto));
        Assert.Multiple(
            () => _mockRoundRepository.Verify(r => r.GetByIdAsync(It.IsAny<RoundId>()), Times.Once),
            () => _mockRoundRepository.Verify(r => r.UpdateAsync(It.IsAny<Round>()), Times.Never)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task SetRoundPoulePositionAsync_CurrentPouleNotInRound_ThrowsArgumentException()
    {
        // Arrange
        var roundId = Guid.NewGuid();
        var prevRoundId = Guid.NewGuid();
        var round = new Round(new RoundId(roundId), RoundName.Create("Round1"), new TournamentId(Guid.NewGuid()));
        var prevRound = new Round(new RoundId(prevRoundId), RoundName.Create("Round0"), new TournamentId(Guid.NewGuid()));
        round.SetPreviousRound(prevRound);
        round.SetSettings(TableTennisRoundSettings.Create(3));

        var currentPouleId = Guid.NewGuid();
        var wrongRoundId = Guid.NewGuid();
        var currentPouleEntity = new Poule(new PouleId(currentPouleId), PouleName.Create("CurrentPoule"), PoulePlayersCount.Create(4), new RoundId(wrongRoundId));
        var currentPouleDto = new PouleDto { Id = currentPouleId, Round = new RoundDto { Id = wrongRoundId } };

        var mockPouleRepository = new Mock<IPouleRepository>();
        mockPouleRepository.Setup(r => r.GetByIdAsync(It.IsAny<PouleId>())).ReturnsAsync(currentPouleEntity);
        _mockMapper.Setup(m => m.Map<PouleDto>(currentPouleEntity)).Returns(currentPouleDto);
        _mockRoundRepository.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync(round);

        var pouleService = new PouleService(_mockMapper.Object, mockPouleRepository.Object, null!);
        var service = new RoundService(_mockMapper.Object, _mockRoundRepository.Object, null!, pouleService);

        var setDto = new SetRoundPoulePositionDto { Id = roundId, CurrentPouleId = currentPouleId, PreviousPouleId = Guid.NewGuid(), CurrentPosition = 1, PreviousPosition = 1 };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.SetRoundPoulePositionAsync(setDto));
        _mockRoundRepository.Verify(r => r.UpdateAsync(It.IsAny<Round>()), Times.Never);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task SetRoundPoulePositionAsync_PreviousPouleNotInPreviousRound_ThrowsArgumentException()
    {
        // Arrange
        var roundId = Guid.NewGuid();
        var prevRoundId = Guid.NewGuid();
        var round = new Round(new RoundId(roundId), RoundName.Create("Round1"), new TournamentId(Guid.NewGuid()));
        var prevRound = new Round(new RoundId(prevRoundId), RoundName.Create("Round0"), new TournamentId(Guid.NewGuid()));
        round.SetPreviousRound(prevRound);
        round.SetSettings(TableTennisRoundSettings.Create(3));

        var currentPouleId = Guid.NewGuid();
        var previousPouleId = Guid.NewGuid();
        var currentPouleEntity = new Poule(new PouleId(currentPouleId), PouleName.Create("CurrentPoule"), PoulePlayersCount.Create(4), new RoundId(roundId));
        var wrongRoundId = Guid.NewGuid();
        var previousPouleEntity = new Poule(new PouleId(previousPouleId), PouleName.Create("PreviousPoule"), PoulePlayersCount.Create(4), new RoundId(wrongRoundId));
        var currentPouleDto = new PouleDto { Id = currentPouleId, Round = new RoundDto { Id = roundId } };
        var previousPouleDto = new PouleDto { Id = previousPouleId, Round = new RoundDto { Id = wrongRoundId } };

        var mockPouleRepository = new Mock<IPouleRepository>();
        mockPouleRepository.Setup(r => r.GetByIdAsync(It.IsAny<PouleId>()))
            .ReturnsAsync((PouleId id) => id.Value == currentPouleId ? currentPouleEntity : previousPouleEntity);
        _mockMapper.Setup(m => m.Map<PouleDto>(currentPouleEntity)).Returns(currentPouleDto);
        _mockMapper.Setup(m => m.Map<PouleDto>(previousPouleEntity)).Returns(previousPouleDto);
        _mockRoundRepository.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync(round);

        var pouleService = new PouleService(_mockMapper.Object, mockPouleRepository.Object, null!);
        var service = new RoundService(_mockMapper.Object, _mockRoundRepository.Object, null!, pouleService);

        var setDto = new SetRoundPoulePositionDto { Id = roundId, CurrentPouleId = currentPouleId, PreviousPouleId = previousPouleId, CurrentPosition = 1, PreviousPosition = 1 };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.SetRoundPoulePositionAsync(setDto));
        _mockRoundRepository.Verify(r => r.UpdateAsync(It.IsAny<Round>()), Times.Never);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task SetRoundPoulePositionAsync_ValidData_RepoUpdateCalledOnce()
    {
        // Arrange
        var roundId = Guid.NewGuid();
        var prevRoundId = Guid.NewGuid();
        var round = new Round(new RoundId(roundId), RoundName.Create("Round1"), new TournamentId(Guid.NewGuid()));
        var prevRound = new Round(new RoundId(prevRoundId), RoundName.Create("Round0"), new TournamentId(Guid.NewGuid()));
        round.SetPreviousRound(prevRound);
        round.SetSettings(TableTennisRoundSettings.Create(3));

        var currentPouleId = Guid.NewGuid();
        var previousPouleId = Guid.NewGuid();
        var currentPouleEntity = new Poule(new PouleId(currentPouleId), PouleName.Create("CurrentPoule"), PoulePlayersCount.Create(4), new RoundId(roundId));
        var previousPouleEntity = new Poule(new PouleId(previousPouleId), PouleName.Create("PreviousPoule"), PoulePlayersCount.Create(4), new RoundId(prevRoundId));
        var currentPouleDto = new PouleDto { Id = currentPouleId, Round = new RoundDto { Id = roundId } };
        var previousPouleDto = new PouleDto { Id = previousPouleId, Round = new RoundDto { Id = prevRoundId } };

        var mockPouleRepository = new Mock<IPouleRepository>();
        mockPouleRepository.Setup(r => r.GetByIdAsync(It.IsAny<PouleId>()))
            .ReturnsAsync((PouleId id) => id.Value == currentPouleId ? currentPouleEntity : previousPouleEntity);
        _mockMapper.Setup(m => m.Map<PouleDto>(currentPouleEntity)).Returns(currentPouleDto);
        _mockMapper.Setup(m => m.Map<PouleDto>(previousPouleEntity)).Returns(previousPouleDto);
        _mockMapper.Setup(m => m.Map<Poule>(currentPouleDto)).Returns(currentPouleEntity);
        _mockMapper.Setup(m => m.Map<Poule>(previousPouleDto)).Returns(previousPouleEntity);
        _mockRoundRepository.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync(round);
        _mockRoundRepository.Setup(r => r.UpdateAsync(It.IsAny<Round>())).Returns(Task.CompletedTask);

        var pouleService = new PouleService(_mockMapper.Object, mockPouleRepository.Object, null!);
        var service = new RoundService(_mockMapper.Object, _mockRoundRepository.Object, null!, pouleService);

        var setDto = new SetRoundPoulePositionDto { Id = roundId, CurrentPouleId = currentPouleId, PreviousPouleId = previousPouleId, CurrentPosition = 1, PreviousPosition = 1 };

        // Act
        await service.SetRoundPoulePositionAsync(setDto);

        // Assert
        _mockRoundRepository.Verify(r => r.UpdateAsync(It.IsAny<Round>()), Times.Once);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task DeleteAsync_ExistingRound_RepoGetByIdAndRemoveCalledOnce()
    {
        // Arrange
        var id = Guid.NewGuid();
        var round = new Round(new RoundId(id), RoundName.Create("Round1"), new TournamentId(Guid.NewGuid()));
        _mockRoundRepository.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync(round);
        _mockRoundRepository.Setup(r => r.RemoveAsync(It.IsAny<Round>())).Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(id);

        // Assert
        Assert.Multiple(
            () => _mockRoundRepository.Verify(r => r.GetByIdAsync(It.IsAny<RoundId>()), Times.Once),
            () => _mockRoundRepository.Verify(r => r.RemoveAsync(It.IsAny<Round>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public async Task DeleteAsync_NonExistingRound_ThrowsArgumentException()
    {
        // Arrange
        _mockRoundRepository.Setup(r => r.GetByIdAsync(It.IsAny<RoundId>())).ReturnsAsync((Round)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.DeleteAsync(Guid.NewGuid()));
        Assert.Multiple(
            () => _mockRoundRepository.Verify(r => r.GetByIdAsync(It.IsAny<RoundId>()), Times.Once),
            () => _mockRoundRepository.Verify(r => r.RemoveAsync(It.IsAny<Round>()), Times.Never)
        );
    }
}
