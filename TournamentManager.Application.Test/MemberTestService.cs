using Moq;
using TournamentManager.Application.Repositories;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class MemberTestService
{
    private readonly Mock<ICrudService<Member>> _mockCrudService;
    private readonly Mock<ITournamentService> _mockParentService;
    private readonly MemberService _service;

    public MemberTestService()
    {
        _mockCrudService = new Mock<ICrudService<Member>>();
        _mockParentService = new Mock<ITournamentService>();
        _service = new MemberService(_mockParentService.Object, _mockCrudService.Object);
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
        _mockCrudService.Setup(crud => crud.GetAll(It.IsAny<Func<Member, bool>>())).Callback(() => {});

        // Act
        _service.GetAll(-1);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.GetAll(It.IsAny<Func<Member, bool>>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void InsertWithValidParentId_CrudInsertWithSpecificsCalledOnce_ParentServiceGetCalledOnce()
    {
        // Arrange
        var newInstance = new Member 
        {
            Name = "Test_Member_Insert"
        };

        _mockCrudService.Setup(crud => crud.Insert(It.IsAny<Member>(), It.IsAny<Action>())).Callback((Member entity, Action action) => {action.Invoke();});
        _mockParentService.Setup(parent => parent.Get(It.IsAny<int>())).Returns(TournamentBuilder.GetSingleTournament());
        
        // Act
        _service.Insert(-1, newInstance);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.Insert(It.IsAny<Member>(), It.IsAny<Action>()), Times.Once),
            () => _mockParentService.Verify(parent => parent.Get(It.IsAny<int>()), Times.Once),
            () => Assert.NotNull(newInstance.Tournament)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void InsertWithInValidParentId_ThrowsNullReferenceException_CrudInsertWithSpecificsCalledOnce_ParentServiceGetCalledOnce()
    {
        // Arrange
        var newInstance = new Member 
        {
            Name = "Test_Member_Insert"
        };

        _mockCrudService.Setup(crud => crud.Insert(It.IsAny<Member>(), It.IsAny<Action>())).Callback((Member entity, Action action) => {action.Invoke();});
        _mockParentService.Setup(parent => parent.Get(It.IsAny<int>())).Returns((Tournament)null);

        Assert.Multiple(
            // Act
            () => Assert.Throws<NullReferenceException>(() => _service.Insert(-1, newInstance)),

            // Assert
            () => _mockCrudService.Verify(crud => crud.Insert(It.IsAny<Member>(), It.IsAny<Action>()), Times.Once),
            () => _mockParentService.Verify(parent => parent.Get(It.IsAny<int>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Update_CrudUpdateCalledOnce()
    {
        // Arrange
        var existingInstance = MemberBuilder.GetSingleMember();
        var updatingInstance = MemberBuilder.GetSingleMember();
        updatingInstance.Name = "Test_Member_Update";

        _mockCrudService.Setup(crud => crud.Update(It.IsAny<int>(), It.IsAny<Member>(), It.IsAny<Action<Member>>())).Callback((int id, Member entity, Action<Member> action) => {action.Invoke(existingInstance);});
        
        // Act
        _service.Update(-1, updatingInstance);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.Update(It.IsAny<int>(), It.IsAny<Member>(), It.IsAny<Action<Member>>()), Times.Once),
            () => Assert.Equal(updatingInstance.Name, existingInstance.Name)
        );
    }
}
