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
    private readonly Mock<ITournamentService> _mockParentService;
    private readonly RoundService _service;

    public RoundTestService()
    {
        _mockCrudService = new Mock<ICrudService<Round>>();
        _mockParentService = new Mock<ITournamentService>();
        _service = new RoundService(_mockParentService.Object, _mockCrudService.Object);
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
            Name = "Test_Round_Insert"
        };

        _mockCrudService.Setup(crud => crud.Insert(It.IsAny<Round>(), It.IsAny<Action>())).Callback((Round entity, Action action) => {action.Invoke();});
        _mockParentService.Setup(parent => parent.Get(It.IsAny<int>())).Returns(TournamentBuilder.GetSingleTournament());
        
        // Act
        _service.Insert(-1, newInstance);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.Insert(It.IsAny<Round>(), It.IsAny<Action>()), Times.Once),
            () => _mockParentService.Verify(parent => parent.Get(It.IsAny<int>()), Times.Once),
            () => Assert.NotNull(newInstance.Tournament)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void InsertWithInValidParentId_ThrowsNullReferenceException_CrudInsertWithSpecificsCalledOnce_ParentServiceGetCalledOnce()
    {
        // Arrange
        var newInstance = new Round 
        {
            Name = "Test_Round_Insert"
        };

        _mockCrudService.Setup(crud => crud.Insert(It.IsAny<Round>(), It.IsAny<Action>())).Callback((Round entity, Action action) => {action.Invoke();});
        _mockParentService.Setup(parent => parent.Get(It.IsAny<int>())).Returns((Tournament)null);

        Assert.Multiple(
            // Act
            () => Assert.Throws<NullReferenceException>(() => _service.Insert(-1, newInstance)),

            // Assert
            () => _mockCrudService.Verify(crud => crud.Insert(It.IsAny<Round>(), It.IsAny<Action>()), Times.Once),
            () => _mockParentService.Verify(parent => parent.Get(It.IsAny<int>()), Times.Once)
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
