using Moq;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class PouleTestService
{
    private readonly Mock<ICrudService<Poule>> _mockCrudService;
    private readonly PouleService _service;

    public PouleTestService()
    {
        _mockCrudService = new Mock<ICrudService<Poule>>();
        _service = new PouleService(_mockCrudService.Object);
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
    public void GetAll_CrudGetAllWithFilterCalledOnce()
    {
        // Arrange
        _mockCrudService.Setup(crud => crud.GetAll(It.IsAny<Func<Poule, bool>>())).Callback(() => {});

        // Act
        _service.GetAll(-1);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.GetAll(It.IsAny<Func<Poule, bool>>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void InsertWithValidParentId_CrudInsertWithSpecificsCalledOnce_ParentServiceGetCalledOnce()
    {
        // Arrange
        var newInstance = new Poule 
        {
            Name = "Test_Poule_Insert",
            RoundId = -1,
        };

        _mockCrudService.Setup(crud => crud.Insert(It.IsAny<Poule>())).Callback(() => {});
        
        // Act
        _service.Insert(newInstance);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.Insert(It.IsAny<Poule>()), Times.Once),
            () => Assert.NotNull(newInstance.RoundId)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Update_ReturnsUpdatedInstance_CrudUpdateCalledOnce()
    {
        // Arrange
        var existingInstance = PouleBuilder.GetSinglePoule();
        var updatingInstance = PouleBuilder.GetSinglePoule();
        updatingInstance.Name = "Test_Poule_Update";

        _mockCrudService.Setup(crud => crud.Update(It.IsAny<int>(), It.IsAny<Poule>(), It.IsAny<Action<Poule>>())).Callback((int id, Poule entity, Action<Poule> action) => {action.Invoke(existingInstance);});
        
        // Act
        _service.Update(-1, updatingInstance);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.Update(It.IsAny<int>(), It.IsAny<Poule>(), It.IsAny<Action<Poule>>()), Times.Once),
            () => Assert.Equal(updatingInstance.Name, existingInstance.Name)
        );
    }
}
