using System.Reflection.Emit;
using Moq;
using TournamentManager.Application.Repositories;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class TournamentTestService
{
    private readonly Mock<ICrudService<Tournament>> _mockCrudService;
    private readonly Mock<ICrudService<TournamentSettings>> _mockSettingsCrudService;
    private readonly Mock<ISportService> _mockSportService;
    private readonly Mock<SportServiceResolver> _mockSportServiceResolver;
    private readonly Mock<ITournamentRepository> _mockTournamentRepository;
    private readonly TournamentService _service;

    public TournamentTestService() 
    {
        _mockCrudService = new Mock<ICrudService<Tournament>>();
        _mockSettingsCrudService = new Mock<ICrudService<TournamentSettings>>();
        _mockSportService = new Mock<ISportService>();
        _mockSportServiceResolver = new Mock<SportServiceResolver>();
        _mockSportServiceResolver.Setup(resolve => resolve(It.IsAny<Sport>())).Returns(_mockSportService.Object);
        _mockTournamentRepository = new Mock<ITournamentRepository>();
        _service = new TournamentService(_mockCrudService.Object, _mockTournamentRepository.Object, _mockSettingsCrudService.Object, _mockSportServiceResolver.Object);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Delete_CrudDeleteCalledOnce()
    {
        // Arrange
        _mockCrudService.Setup(crud => crud.Delete(It.IsAny<int>())).Callback(() => {});

        // Act
        _service.Delete(-1);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.Delete(It.IsAny<int>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Get_CrudGetCalledOnce()
    {
        // Arrange
        _mockCrudService.Setup(crud => crud.Get(It.IsAny<int>())).Callback(() => {});

        // Act
        _service.Get(-1);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.Get(It.IsAny<int>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetAll_CrudGetAllCalledOnce()
    {
        // Arrange
        _mockCrudService.Setup(crud => crud.GetAll()).Callback(() => {});

        // Act
        _service.GetAll();

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.GetAll(), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Insert_CrudInsertCalledOnce()
    {
        // Arrange
        _mockCrudService.Setup(crud => crud.Insert(It.IsAny<Tournament>())).Callback(() => {});

        // Act
        _service.Insert(TournamentBuilder.GetSingleTournament());

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.Insert(It.IsAny<Tournament>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Update_CrudUpdateCalledOnce()
    {
        // Arrange
        _mockCrudService.Setup(crud => crud.Update(It.IsAny<int>(), It.IsAny<Tournament>(), It.IsAny<Action<Tournament>>())).Callback(() => {});
        
        // Act
        _service.Update(-1, TournamentBuilder.GetSingleTournament());

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.Update(It.IsAny<int>(), It.IsAny<Tournament>(), It.IsAny<Action<Tournament>>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void SetSettings_TournamentRepoGetWithSettingsCalledOnce_SettingsCrudInsertCalledOnce_SettingsCrudDeleteCalledNever()
    {
        // Arrange
        var tournament = TournamentBuilder.GetSingleTournament();
        var settings = TournamentSettingsBuilder.GetSingleTournamentSettings<TableTennisSettings>(tournament.Id.Value);

        _mockTournamentRepository.Setup(repo => repo.GetWithSettings(It.IsAny<int>())).Returns(tournament);
        _mockSettingsCrudService.Setup(crud => crud.Insert(It.IsAny<TournamentSettings>())).Callback(() => {});

        // Act
        _service.SetSettings(settings);

        // Assert
        Assert.Multiple(
            () => _mockTournamentRepository.Verify(repo => repo.GetWithSettings(It.IsAny<int>()), Times.Once),
            () => _mockSettingsCrudService.Verify(crud => crud.Insert(It.IsAny<TournamentSettings>()), Times.Once),
            () => _mockSettingsCrudService.Verify(crud => crud.Delete(It.IsAny<int>()), Times.Never)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void SetSettingsForOverwrite_TournamentRepoGetWithSettingsCalledOnce_SettingsCrudInsertCalledOnce_SettingsCrudDeleteCalledOnce()
    {
        // Arrange
        var tournament = TournamentBuilder.GetSingleTournament();
        tournament.Settings = TournamentSettingsBuilder.GetSingleTournamentSettings<TableTennisSettings>(1, tournament.Id.Value);
        var settings = TournamentSettingsBuilder.GetSingleTournamentSettings<TableTennisSettings>(2, tournament.Id.Value);

        _mockTournamentRepository.Setup(repo => repo.GetWithSettings(It.IsAny<int>())).Returns(tournament);
        _mockSettingsCrudService.Setup(crud => crud.Insert(It.IsAny<TournamentSettings>())).Callback(() => {});

        // Act
        _service.SetSettings(settings);

        // Assert
        Assert.Multiple(
            () => _mockTournamentRepository.Verify(repo => repo.GetWithSettings(It.IsAny<int>()), Times.Once),
            () => _mockSettingsCrudService.Verify(crud => crud.Insert(It.IsAny<TournamentSettings>()), Times.Once),
            () => _mockSettingsCrudService.Verify(crud => crud.Delete(It.IsAny<int>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void SetSettingsWithNonExistingTournament_ThrowsArgumentException_TournamentRepoGetWithSettingsCalledOnce_SettingsCrudInsertCalledNever_SettingsCrudDeleteCalledNever()
    {
        // Arrange
        var settings = TournamentSettingsBuilder.GetSingleTournamentSettings<TableTennisSettings>();

        _mockTournamentRepository.Setup(repo => repo.GetWithSettings(It.IsAny<int>())).Returns((Tournament)null);

        // Act & Assert
        Assert.Multiple(
            () => Assert.Throws<ArgumentException>(() => _service.SetSettings(settings)),
            () => _mockTournamentRepository.Verify(repo => repo.GetWithSettings(It.IsAny<int>()), Times.Once),
            () => _mockSettingsCrudService.Verify(crud => crud.Insert(It.IsAny<TournamentSettings>()), Times.Never),
            () => _mockSettingsCrudService.Verify(crud => crud.Delete(It.IsAny<int>()), Times.Never)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GenerateWithValidId_ResolvesSportService_GenerateCalledOnce()
    {
        // Arrange
        var tournament = TournamentBuilder.GetSingleTournament();
        tournament.Sport = Sport.TableTennis;
        Tournament forwardedTournament = null!;

        _mockTournamentRepository.Setup(repo => repo.GetWithAllRelations(It.IsAny<int>())).Returns(tournament);
        _mockSportService.Setup(sport => sport.Generate(It.IsAny<Tournament>())).Callback<Tournament>(tournament => {forwardedTournament = tournament;});

        // Act
        _service.Generate(-1);

        // Assert
        Assert.Multiple(
            () => _mockTournamentRepository.Verify(repo => repo.GetWithAllRelations(It.IsAny<int>()), Times.Once),
            () => _mockSportService.Verify(sport => sport.Generate(It.IsAny<Tournament>()), Times.Once),
            () => Assert.NotNull(forwardedTournament),
            () => Assert.Equal(tournament, forwardedTournament)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GenerateWithInValidId_ThrowsArgumentException_GenerateCalledNever()
    {
        // Arrange
        _mockTournamentRepository.Setup(repo => repo.GetWithAllRelations(It.IsAny<int>())).Returns((Tournament)null!);

        // Assert & Act
        Assert.Multiple(
            () => Assert.Throws<ArgumentException>(() => _service.Generate(-1)),
            () => _mockTournamentRepository.Verify(repo => repo.GetWithAllRelations(It.IsAny<int>()), Times.Once),
            () => _mockSportService.Verify(sport => sport.Generate(It.IsAny<Tournament>()), Times.Never)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetSettings_ExistingTournament_RepoGetWithSettingsCalledOnce()
    {
        // arrange
        var tournament = TournamentBuilder.GetSingleTournament(1);
        tournament.Settings = TournamentSettingsBuilder.GetSingleTournamentSettings<TableTennisSettings>(1, 1);
        _mockTournamentRepository.Setup(repo => repo.GetWithSettings(It.IsAny<int>())).Returns(tournament);

        // act
        _service.GetSettings(-1);

        // assert
        Assert.Multiple(
            () => _mockTournamentRepository.Verify(repo => repo.GetWithSettings(It.IsAny<int>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetSettings_NonExistingTournament_RepoGetWithSettingsCalledOnce_ThrowsArgumentException()
    {
        // arrange
        _mockTournamentRepository.Setup(repo => repo.GetWithSettings(It.IsAny<int>())).Returns((Tournament)null!);

        Assert.Multiple(
            // act
            () => Assert.Throws<ArgumentException>(() => _service.GetSettings(-1)),

            // assert
            () => _mockTournamentRepository.Verify(repo => repo.GetWithSettings(It.IsAny<int>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetNonExistingSettings_ExistingTournament_RepoGetWithSettingsCalledOnce_ThrowsArgumentException()
    {
        // arrange
        var tournament = TournamentBuilder.GetSingleTournament(1);
        _mockTournamentRepository.Setup(repo => repo.GetWithSettings(It.IsAny<int>())).Returns(tournament);

        Assert.Multiple(
            // act
            () => Assert.Throws<ArgumentException>(() => _service.GetSettings(-1)),

            // assert
            () => _mockTournamentRepository.Verify(repo => repo.GetWithSettings(It.IsAny<int>()), Times.Once)
        );
    }
}