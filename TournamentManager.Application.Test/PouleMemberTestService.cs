using Moq;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class PouleMemberTestService
{
    private Mock<IPouleService> _mockPouleService;
    private Mock<IMemberService> _mockMemberService;
    private Mock<ICrudService<PouleMember>> _mockCrudService;
    private PouleMemberService _service;

    public PouleMemberTestService()
    {
        _mockPouleService = new Mock<IPouleService>();
        _mockMemberService = new Mock<IMemberService>();
        _mockCrudService = new Mock<ICrudService<PouleMember>>();
        _service = new PouleMemberService(_mockCrudService.Object, _mockPouleService.Object, _mockMemberService.Object);
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
    public void GetAll_CrudGetAllWithFilterCalledOnce()
    {
        // Arrange
        _mockCrudService.Setup(crud => crud.GetAll(It.IsAny<Func<PouleMember, bool>>())).Callback(() => {});

        // Act
        _service.GetAll(-1);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.GetAll(It.IsAny<Func<PouleMember, bool>>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void CreateWithValidIds_CrudInsertCalledOnce_PouleServiceGetCalledOnce_MemberServiceGetCalledOnce()
    {
        // Arrange
        _mockCrudService.Setup(crud => crud.Insert(It.IsAny<PouleMember>(), null)).Callback(() => {});
        _mockPouleService.Setup(parent => parent.Get(It.IsAny<int>())).Returns(PouleBuilder.GetSinglePoule(1));
        _mockMemberService.Setup(parent => parent.Get(It.IsAny<int>())).Returns(MemberBuilder.GetSingleMember(1));
        
        // Act
        var newInstance = _service.Create(-1, -1);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.Insert(It.IsAny<PouleMember>(), It.IsAny<Action>()), Times.Once),
            () => _mockPouleService.Verify(parent => parent.Get(It.IsAny<int>()), Times.Once),
            () => _mockMemberService.Verify(parent => parent.Get(It.IsAny<int>()), Times.Once),
            () => Assert.NotNull(newInstance.Poule),
            () => Assert.NotNull(newInstance.Member)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void CreateWithInValidPouleId_ThrowsNullReferenceException_CrudInsertCalledNever_PouleServiceGetCalledOnce_MemberServiceGetCalledNever()
    {
        // Arrange
        _mockCrudService.Setup(crud => crud.Insert(It.IsAny<PouleMember>(), null)).Callback(() => {});
        _mockPouleService.Setup(parent => parent.Get(It.IsAny<int>())).Returns((Poule)null);
        _mockMemberService.Setup(parent => parent.Get(It.IsAny<int>())).Returns(MemberBuilder.GetSingleMember());
        
        Assert.Multiple(
            // Act
            () => Assert.Throws<NullReferenceException>(() => _service.Create(-1, -1)),

            // Assert
            () => _mockCrudService.Verify(crud => crud.Insert(It.IsAny<PouleMember>(), It.IsAny<Action>()), Times.Never),
            () => _mockPouleService.Verify(parent => parent.Get(It.IsAny<int>()), Times.Once),
            () => _mockMemberService.Verify(parent => parent.Get(It.IsAny<int>()), Times.Never)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void CreateWithInValidMemberId_ThrowsNullReferenceException_CrudInsertCalledNever_PouleServiceGetCalledOnce_MemberServiceGetCalledOnce()
    {
        // Arrange
        _mockCrudService.Setup(crud => crud.Insert(It.IsAny<PouleMember>(), null)).Callback(() => {});
        _mockPouleService.Setup(parent => parent.Get(It.IsAny<int>())).Returns(PouleBuilder.GetSinglePoule());
        _mockMemberService.Setup(parent => parent.Get(It.IsAny<int>())).Returns((Member)null);
        
        Assert.Multiple(
            // Act
            () => Assert.Throws<NullReferenceException>(() => _service.Create(-1, -1)),

            // Assert
            () => _mockCrudService.Verify(crud => crud.Insert(It.IsAny<PouleMember>(), It.IsAny<Action>()), Times.Never),
            () => _mockPouleService.Verify(parent => parent.Get(It.IsAny<int>()), Times.Once),
            () => _mockMemberService.Verify(parent => parent.Get(It.IsAny<int>()), Times.Once)
        );
    }
}
