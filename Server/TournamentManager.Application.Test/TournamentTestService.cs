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
    private readonly TournamentService _service;

    public TournamentTestService() 
    {
        _mockCrudService = new Mock<ICrudService<Tournament>>();
        _mockSettingsCrudService = new Mock<ICrudService<TournamentSettings>>();
        _service = new TournamentService(_mockCrudService.Object, _mockSettingsCrudService.Object);
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
    public void SetSettings_CrudGetCalledOnce_SettingsCrudInsertCalledOnce()
    {
        // Arrange
        var tournament = TournamentBuilder.GetSingleTournament();
        var settings = TournamentSettingsBuilder.GetSingleTournamentSettings<TableTennisSettings>(tournament.Id.Value);

        _mockCrudService.Setup(crud => crud.Get(It.IsAny<int>())).Returns(tournament);
        _mockSettingsCrudService.Setup(crud => crud.Insert(It.IsAny<TournamentSettings>())).Callback(() => {});

        // Act
        _service.SetSettings(settings);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.Get(It.IsAny<int>()), Times.Once),
            () => _mockSettingsCrudService.Verify(crud => crud.Insert(It.IsAny<TournamentSettings>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void SetSettingsWithNonExistingTournament_ThrowsArgumentException_CrudGetCalledOnce_SettingsCrudInsertCalledNever()
    {
        // Arrange
        var settings = TournamentSettingsBuilder.GetSingleTournamentSettings<TableTennisSettings>();

        _mockCrudService.Setup(crud => crud.Get(It.IsAny<int>())).Returns((Tournament)null);

        // Act & Assert
        Assert.Multiple(
            () => Assert.Throws<ArgumentException>(() => _service.SetSettings(settings)),
            () => _mockCrudService.Verify(crud => crud.Get(It.IsAny<int>()), Times.Once),
            () => _mockSettingsCrudService.Verify(crud => crud.Insert(It.IsAny<TournamentSettings>()), Times.Never)
        );
    }
}