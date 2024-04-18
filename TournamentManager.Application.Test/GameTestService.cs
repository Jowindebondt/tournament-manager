using Moq;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class GameTestService
{
    private readonly Mock<ICrudService<Game>> _mockCrudService;
    private readonly Mock<IMatchService> _mockParentService;
    private readonly GameService _service;

    public GameTestService()
    {
        _mockCrudService = new Mock<ICrudService<Game>>();
        _mockParentService = new Mock<IMatchService>();
        _service = new GameService(_mockParentService.Object, _mockCrudService.Object);
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
        _mockCrudService.Setup(crud => crud.GetAll(It.IsAny<Func<Game, bool>>())).Callback(() => {});

        // Act
        _service.GetAll(-1);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.GetAll(It.IsAny<Func<Game, bool>>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void InsertWithValidParentId_CrudInsertWithSpecificsCalledOnce_ParentServiceGetCalledOnce()
    {
        // Arrange
        var newInstance = new Game 
        {
            Score_1 = 0,
            Score_2 = 0,
        };

        _mockCrudService.Setup(crud => crud.Insert(It.IsAny<Game>(), It.IsAny<Action>())).Callback((Game entity, Action action) => {action.Invoke();});
        _mockParentService.Setup(parent => parent.Get(It.IsAny<int>())).Returns(MatchBuilder.GetSingleMatch());
        
        // Act
        _service.Insert(-1, newInstance);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.Insert(It.IsAny<Game>(), It.IsAny<Action>()), Times.Once),
            () => _mockParentService.Verify(parent => parent.Get(It.IsAny<int>()), Times.Once),
            () => Assert.NotNull(newInstance.Match)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void InsertWithInValidParentId_ThrowsNullReferenceException_CrudInsertWithSpecificsCalledOnce_ParentServiceGetCalledOnce()
    {
        // Arrange
        var newInstance = new Game 
        {
            Score_1 = 0,
            Score_2 = 0,
        };

        _mockCrudService.Setup(crud => crud.Insert(It.IsAny<Game>(), It.IsAny<Action>())).Callback((Game entity, Action action) => {action.Invoke();});
        _mockParentService.Setup(parent => parent.Get(It.IsAny<int>())).Returns((Domain.Match)null);

        Assert.Multiple(
            // Act
            () => Assert.Throws<NullReferenceException>(() => _service.Insert(-1, newInstance)),

            // Assert
            () => _mockCrudService.Verify(crud => crud.Insert(It.IsAny<Game>(), It.IsAny<Action>()), Times.Once),
            () => _mockParentService.Verify(parent => parent.Get(It.IsAny<int>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Update_ReturnsUpdatedInstance_CrudUpdateCalledOnce()
    {
        // Arrange
        var existingInstance = GameBuilder.GetSingleGame();
        var updatingInstance = GameBuilder.GetSingleGame();
        updatingInstance.Score_1 = 11;
        updatingInstance.Score_2 = 9;

        _mockCrudService.Setup(crud => crud.Update(It.IsAny<int>(), It.IsAny<Game>(), It.IsAny<Action<Game>>())).Callback((int id, Game entity, Action<Game> action) => {action.Invoke(existingInstance);});
        
        // Act
        _service.Update(-1, updatingInstance);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.Update(It.IsAny<int>(), It.IsAny<Game>(), It.IsAny<Action<Game>>()), Times.Once),
            () => Assert.Equal(updatingInstance.Score_1, existingInstance.Score_1),
            () => Assert.Equal(updatingInstance.Score_2, existingInstance.Score_2)
        );
    }
}
