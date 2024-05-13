using Moq;
using TournamentManager.Application.Repositories;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class RoundTestService
{
    private readonly Mock<ICrudService<Round>> _mockCrudService;
    private readonly RoundService _service;

    public RoundTestService()
    {
        _mockCrudService = new Mock<ICrudService<Round>>();
        _service = new RoundService(_mockCrudService.Object);
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
        _mockCrudService.Setup(crud => crud.GetAll(It.IsAny<Func<Round, bool>>())).Callback(() => {});

        // Act
        _service.GetAll(-1);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.GetAll(It.IsAny<Func<Round, bool>>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void InsertWithValidParentId_CrudInsertWithSpecificsCalledOnce_ParentServiceGetCalledOnce()
    {
        // Arrange
        var newInstance = new Round 
        {
            Name = "Test_Round_Insert",
            TournamentId = -1,
        };

        _mockCrudService.Setup(crud => crud.Insert(It.IsAny<Round>(), null)).Callback(() => {});
        
        // Act
        _service.Insert(newInstance);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.Insert(It.IsAny<Round>(), null), Times.Once),
            () => Assert.NotNull(newInstance.TournamentId)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Update_CrudUpdateCalledOnce()
    {
        // Arrange
        var existingInstance = RoundBuilder.GetSingleRound();
        var updatingInstance = RoundBuilder.GetSingleRound();
        updatingInstance.Name = "Test_Round_Update";

        _mockCrudService.Setup(crud => crud.Update(It.IsAny<int>(), It.IsAny<Round>(), It.IsAny<Action<Round>>())).Callback((int id, Round entity, Action<Round> action) => {action.Invoke(existingInstance);});
        
        // Act
        _service.Update(-1, updatingInstance);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.Update(It.IsAny<int>(), It.IsAny<Round>(), It.IsAny<Action<Round>>()), Times.Once),
            () => Assert.Equal(updatingInstance.Name, existingInstance.Name)
        );
    }
}
