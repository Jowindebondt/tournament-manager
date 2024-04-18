using Moq;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class PouleMemberTestService
{
    private Mock<IPouleService> _mockPouleService;
    private Mock<IPlayerService> _mockPlayerService;
    private Mock<ICrudService<PoulePlayer>> _mockCrudService;
    private PoulePlayerService _service;

    public PouleMemberTestService()
    {
        _mockPouleService = new Mock<IPouleService>();
        _mockPlayerService = new Mock<IPlayerService>();
        _mockCrudService = new Mock<ICrudService<PoulePlayer>>();
        _service = new PoulePlayerService(_mockCrudService.Object, _mockPouleService.Object, _mockPlayerService.Object);
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
        _mockCrudService.Setup(crud => crud.GetAll(It.IsAny<Func<PoulePlayer, bool>>())).Callback(() => {});

        // Act
        _service.GetAll(-1);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.GetAll(It.IsAny<Func<PoulePlayer, bool>>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void CreateWithValidIds_CrudInsertCalledOnce_PouleServiceGetCalledOnce_MemberServiceGetCalledOnce()
    {
        // Arrange
        _mockCrudService.Setup(crud => crud.Insert(It.IsAny<PoulePlayer>(), null)).Callback(() => {});
        _mockPouleService.Setup(parent => parent.Get(It.IsAny<int>())).Returns(PouleBuilder.GetSinglePoule(1));
        _mockPlayerService.Setup(parent => parent.Get(It.IsAny<int>())).Returns(PlayerBuilder.GetSinglePlayer(1));
        
        // Act
        var newInstance = _service.Create(-1, -1);

        // Assert
        Assert.Multiple(
            () => _mockCrudService.Verify(crud => crud.Insert(It.IsAny<PoulePlayer>(), It.IsAny<Action>()), Times.Once),
            () => _mockPouleService.Verify(parent => parent.Get(It.IsAny<int>()), Times.Once),
            () => _mockPlayerService.Verify(parent => parent.Get(It.IsAny<int>()), Times.Once),
            () => Assert.NotNull(newInstance.Poule),
            () => Assert.NotNull(newInstance.Player)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void CreateWithInValidPouleId_ThrowsNullReferenceException_CrudInsertCalledNever_PouleServiceGetCalledOnce_MemberServiceGetCalledNever()
    {
        // Arrange
        _mockCrudService.Setup(crud => crud.Insert(It.IsAny<PoulePlayer>(), null)).Callback(() => {});
        _mockPouleService.Setup(parent => parent.Get(It.IsAny<int>())).Returns((Poule)null);
        _mockPlayerService.Setup(parent => parent.Get(It.IsAny<int>())).Returns(PlayerBuilder.GetSinglePlayer());
        
        Assert.Multiple(
            // Act
            () => Assert.Throws<NullReferenceException>(() => _service.Create(-1, -1)),

            // Assert
            () => _mockCrudService.Verify(crud => crud.Insert(It.IsAny<PoulePlayer>(), It.IsAny<Action>()), Times.Never),
            () => _mockPouleService.Verify(parent => parent.Get(It.IsAny<int>()), Times.Once),
            () => _mockPlayerService.Verify(parent => parent.Get(It.IsAny<int>()), Times.Never)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void CreateWithInValidMemberId_ThrowsNullReferenceException_CrudInsertCalledNever_PouleServiceGetCalledOnce_MemberServiceGetCalledOnce()
    {
        // Arrange
        _mockCrudService.Setup(crud => crud.Insert(It.IsAny<PoulePlayer>(), null)).Callback(() => {});
        _mockPouleService.Setup(parent => parent.Get(It.IsAny<int>())).Returns(PouleBuilder.GetSinglePoule());
        _mockPlayerService.Setup(parent => parent.Get(It.IsAny<int>())).Returns((Player)null);
        
        Assert.Multiple(
            // Act
            () => Assert.Throws<NullReferenceException>(() => _service.Create(-1, -1)),

            // Assert
            () => _mockCrudService.Verify(crud => crud.Insert(It.IsAny<PoulePlayer>(), It.IsAny<Action>()), Times.Never),
            () => _mockPouleService.Verify(parent => parent.Get(It.IsAny<int>()), Times.Once),
            () => _mockPlayerService.Verify(parent => parent.Get(It.IsAny<int>()), Times.Once)
        );
    }
}
