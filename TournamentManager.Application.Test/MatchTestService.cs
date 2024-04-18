using Moq;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class MatchTestService
{
    private readonly Mock<ICrudService<Domain.Match>> _mockCrudService;
    private readonly Mock<IPouleService> _mockParentService;
    private readonly MatchService _service;

    public MatchTestService()
    {
        _mockCrudService = new Mock<ICrudService<Domain.Match>>();
        _mockParentService = new Mock<IPouleService>();  
        _service = new MatchService(_mockParentService.Object, _mockCrudService.Object);  
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
        _mockCrudService.Setup(crud => crud.GetAll(It.IsAny<Func<Domain.Match, bool>>())).Callback(() => {});

        // Act
        _service.GetAll(-1);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.GetAll(It.IsAny<Func<Domain.Match, bool>>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void InsertWithValidParentId_CrudInsertWithSpecificsCalledOnce_ParentServiceGetCalledOnce()
    {
        // Arrange
        var newInstance = new Domain.Match 
        {
            Player_1 = MemberBuilder.GetSingleMember(),
            Player_2 = MemberBuilder.GetSingleMember(),
        };

        _mockCrudService.Setup(crud => crud.Insert(It.IsAny<Domain.Match>(), It.IsAny<Action>())).Callback((Domain.Match entity, Action action) => {action.Invoke();});
        _mockParentService.Setup(parent => parent.Get(It.IsAny<int>())).Returns(PouleBuilder.GetSinglePoule());
        
        // Act
        _service.Insert(-1, newInstance);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.Insert(It.IsAny<Domain.Match>(), It.IsAny<Action>()), Times.Once),
            () => _mockParentService.Verify(parent => parent.Get(It.IsAny<int>()), Times.Once),
            () => Assert.NotNull(newInstance.Poule)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void InsertWithInValidParentId_ThrowsNullReferenceException_CrudInsertWithSpecificsCalledOnce_ParentServiceGetCalledOnce()
    {
        // Arrange
        var newInstance = new Domain.Match 
        {
            Player_1 = MemberBuilder.GetSingleMember(),
            Player_2 = MemberBuilder.GetSingleMember(),
        };

        _mockCrudService.Setup(crud => crud.Insert(It.IsAny<Domain.Match>(), It.IsAny<Action>())).Callback((Domain.Match entity, Action action) => {action.Invoke();});
        _mockParentService.Setup(parent => parent.Get(It.IsAny<int>())).Returns((Poule)null);

        Assert.Multiple(
            // Act
            () => Assert.Throws<NullReferenceException>(() => _service.Insert(-1, newInstance)),

            // Assert
            () => _mockCrudService.Verify(crud => crud.Insert(It.IsAny<Domain.Match>(), It.IsAny<Action>()), Times.Once),
            () => _mockParentService.Verify(parent => parent.Get(It.IsAny<int>()), Times.Once)
        );
    }
}
