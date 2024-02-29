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
    private readonly TournamentService _service;

    public TournamentTestService() 
    {
        _mockCrudService = new Mock<ICrudService<Tournament>>();
        _service = new TournamentService(_mockCrudService.Object);
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
        _mockCrudService.Setup(crud => crud.Insert(It.IsAny<Tournament>(), null)).Callback(() => {});

        // Act
        _service.Insert(TournamentBuilder.GetSingleTournament());

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.Insert(It.IsAny<Tournament>(), null), Times.Once)
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
}