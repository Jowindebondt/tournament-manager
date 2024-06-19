using System.Runtime.CompilerServices;
using Moq;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class PouleTestService
{
    private readonly Mock<ICrudService<Poule>> _mockCrudService;
    private readonly Mock<IMemberService> _mockMemberService;
    private readonly Mock<IPlayerService> _mockPlayerService;
    private readonly Mock<IPouleRepository> _mockPouleRepository;
    private readonly PouleService _service;

    public PouleTestService()
    {
        _mockCrudService = new Mock<ICrudService<Poule>>();
        _mockMemberService = new Mock<IMemberService>();
        _mockPlayerService = new Mock<IPlayerService>();
        _mockPouleRepository = new Mock<IPouleRepository>();
        _service = new PouleService(_mockCrudService.Object, _mockMemberService.Object, _mockPlayerService.Object, _mockPouleRepository.Object);
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

    [Theory]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(10)]
    public void AddValidMembers_CreatesSameAmountPlayers(int amount)
    {
        // Arrange
        var poule = PouleBuilder.GetSinglePoule(1, 1);
        poule.Round = RoundBuilder.GetSingleRound(1, 1);
        
        var playerId = -1;
        var members = new Member[amount];
        for (int i = 0; i < amount; i++)
        {
            var id = i + 1;
            var member = MemberBuilder.GetSingleMember(id, 1);
            members[i] = member;
            _mockMemberService.Setup(mm => mm.Get(member.Id.Value)).Returns(member);
        }

        _mockPouleRepository.Setup(repo => repo.GetWithAncestors(It.IsAny<int>())).Returns(poule);
        _mockMemberService.Setup(member => member.Update(It.IsAny<int>(), It.IsAny<Member>())).Callback((int id, Member member) => {});
        _mockPlayerService.Setup(player => player.Insert(It.IsAny<Player>())).Callback((Player player) => {
            player.Id = playerId;
            playerId--; 
        });

        // Act
        _service.AddMembers(poule.Id.Value, members.Select(u => u.Id.Value));

        // Assert
        Assert.Multiple(
            () => _mockPouleRepository.Verify(repo => repo.GetWithAncestors(It.IsAny<int>()), Times.Once),
            () => _mockMemberService.Verify(member => member.Get(It.IsAny<int>()), Times.Exactly(amount)),
            () => _mockPlayerService.Verify(player => player.Insert(It.IsAny<Player>()), Times.Exactly(amount)),
            () => _mockMemberService.Verify(member => member.Update(It.IsAny<int>(), It.IsAny<Member>()), Times.Exactly(amount)),
            () => Assert.All(members, (member) => Assert.NotNull(member.PlayerId))
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void AddValidMembers_DifferentTournament_ThrowsArgumentException()
    {
        // Arrange
        var poule = PouleBuilder.GetSinglePoule(1, 1);
        poule.Round = RoundBuilder.GetSingleRound(1, 1);
        var member = MemberBuilder.GetSingleMember(1, 2);

        _mockPouleRepository.Setup(repo => repo.GetWithAncestors(It.IsAny<int>())).Returns(poule);
        _mockMemberService.Setup(mm => mm.Get(It.IsAny<int>())).Returns(member);

        // Act & Assert
        Assert.Multiple(
            () => Assert.Throws<ArgumentException>(() => _service.AddMembers(poule.Id.Value, [member.Id.Value])),
            () => _mockPouleRepository.Verify(repo => repo.GetWithAncestors(It.IsAny<int>()), Times.Once),
            () => _mockMemberService.Verify(member => member.Get(It.IsAny<int>()), Times.Once),
            () => _mockPlayerService.Verify(player => player.Insert(It.IsAny<Player>()), Times.Never),
            () => _mockMemberService.Verify(member => member.Update(It.IsAny<int>(), It.IsAny<Member>()), Times.Never)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void AddValidMembers_MixWithSameAndDifferentTournament_ThrowsArgumentException_CreatesPlayers()
    {
        // Arrange
        var poule = PouleBuilder.GetSinglePoule(1, 1);
        poule.Round = RoundBuilder.GetSingleRound(1, 1);

        var member1 = MemberBuilder.GetSingleMember(1, 1);
        var member2 = MemberBuilder.GetSingleMember(2, 1);
        var member3 = MemberBuilder.GetSingleMember(3, 2);
        var member4 = MemberBuilder.GetSingleMember(4, 2);
        
        var playerId = -1;

        _mockPouleRepository.Setup(repo => repo.GetWithAncestors(It.IsAny<int>())).Returns(poule);
        _mockMemberService.Setup(mm => mm.Get(-1)).Returns(member1);
        _mockMemberService.Setup(mm => mm.Get(-2)).Returns(member2);
        _mockMemberService.Setup(mm => mm.Get(-3)).Returns(member3);
        _mockMemberService.Setup(mm => mm.Get(-4)).Returns(member4);
        _mockMemberService.Setup(mm => mm.Get(-5)).Returns((Member)null);
        _mockPlayerService.Setup(player => player.Insert(It.IsAny<Player>())).Callback((Player player) => {
            player.Id = playerId;
            playerId--; 
        });

        // Act & Assert
        Assert.Multiple(
            () => Assert.Throws<ArgumentException>(() => _service.AddMembers(poule.Id.Value, [member1.Id.Value, member2.Id.Value, member3.Id.Value, member4.Id.Value, -5])),
            () => _mockPouleRepository.Verify(repo => repo.GetWithAncestors(It.IsAny<int>()), Times.Once),
            () => _mockMemberService.Verify(member => member.Get(It.IsAny<int>()), Times.Exactly(5)),
            () => _mockPlayerService.Verify(player => player.Insert(It.IsAny<Player>()), Times.Exactly(2)),
            () => _mockMemberService.Verify(member => member.Update(It.IsAny<int>(), It.IsAny<Member>()), Times.Exactly(2)),
            () => Assert.NotNull(member1.PlayerId),
            () => Assert.NotNull(member2.PlayerId)
        );
    }

    [Theory]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(10)]
    public void AddValidMembersAsTeam_CreatesPlayerWithAllMembers(int amount)
    {
        // Arrange
        var poule = PouleBuilder.GetSinglePoule(1, 1);
        poule.Round = RoundBuilder.GetSingleRound(1, 1);
        
        var members = new Member[amount];
        for (int i = 0; i < amount; i++)
        {
            var id = i + 1;
            var member = MemberBuilder.GetSingleMember(id, 1);
            members[i] = member;
            _mockMemberService.Setup(mm => mm.Get(member.Id.Value)).Returns(member);
        }

        _mockPouleRepository.Setup(repo => repo.GetWithAncestors(It.IsAny<int>())).Returns(poule);
        _mockMemberService.Setup(member => member.Update(It.IsAny<int>(), It.IsAny<Member>())).Callback((int id, Member member) => {});
        _mockPlayerService.Setup(player => player.Insert(It.IsAny<Player>())).Callback((Player player) => {
            player.Id = -1;
        });

        // Act
        _service.AddMembersAsTeam(poule.Id.Value, members.Select(u => u.Id.Value));

        // Assert
        Assert.Multiple(
            () => _mockPouleRepository.Verify(repo => repo.GetWithAncestors(It.IsAny<int>()), Times.Once),
            () => _mockMemberService.Verify(member => member.Get(It.IsAny<int>()), Times.Exactly(amount)),
            () => _mockPlayerService.Verify(player => player.Insert(It.IsAny<Player>()), Times.Once),
            () => _mockMemberService.Verify(member => member.Update(It.IsAny<int>(), It.IsAny<Member>()), Times.Exactly(amount)),
            () => Assert.All(members, (member) => Assert.Equal(-1, member.PlayerId))
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void AddValidMembersAsTeam_DifferentTournament_ThrowsArgumentException()
    {
        // Arrange
        var poule = PouleBuilder.GetSinglePoule(1, 1);
        poule.Round = RoundBuilder.GetSingleRound(1, 1);
        var member = MemberBuilder.GetSingleMember(1, 2);

        _mockPouleRepository.Setup(repo => repo.GetWithAncestors(It.IsAny<int>())).Returns(poule);
        _mockMemberService.Setup(mm => mm.Get(It.IsAny<int>())).Returns(member);
        _mockPlayerService.Setup(player => player.Insert(It.IsAny<Player>())).Callback((Player player) => {
            player.Id = -1;
        });
        _mockPlayerService.Setup(player => player.Delete(It.IsAny<int>())).Callback(() => {});

        // Act & Assert
        Assert.Multiple(
            () => Assert.Throws<ArgumentException>(() => _service.AddMembersAsTeam(poule.Id.Value, [member.Id.Value])),
            () => _mockPouleRepository.Verify(repo => repo.GetWithAncestors(It.IsAny<int>()), Times.Once),
            () => _mockMemberService.Verify(member => member.Get(It.IsAny<int>()), Times.Once),
            () => _mockPlayerService.Verify(player => player.Insert(It.IsAny<Player>()), Times.Once),
            () => _mockPlayerService.Verify(player => player.Delete(It.IsAny<int>()), Times.Once),
            () => _mockMemberService.Verify(member => member.Update(It.IsAny<int>(), It.IsAny<Member>()), Times.Never)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void AddValidMembersAsTeam_MixWithSameAndDifferentTournament_ThrowsArgumentException_CreatesPlayerWithTournamentMembers()
    {
        // Arrange
        var poule = PouleBuilder.GetSinglePoule(1, 1);
        poule.Round = RoundBuilder.GetSingleRound(1, 1);

        var member1 = MemberBuilder.GetSingleMember(1, 1);
        var member2 = MemberBuilder.GetSingleMember(2, 1);
        var member3 = MemberBuilder.GetSingleMember(3, 2);
        var member4 = MemberBuilder.GetSingleMember(4, 2);
        
        _mockPouleRepository.Setup(repo => repo.GetWithAncestors(It.IsAny<int>())).Returns(poule);
        _mockMemberService.Setup(mm => mm.Get(-1)).Returns(member1);
        _mockMemberService.Setup(mm => mm.Get(-2)).Returns(member2);
        _mockMemberService.Setup(mm => mm.Get(-3)).Returns(member3);
        _mockMemberService.Setup(mm => mm.Get(-4)).Returns(member4);
        _mockMemberService.Setup(mm => mm.Get(-5)).Returns((Member)null);
        _mockPlayerService.Setup(player => player.Insert(It.IsAny<Player>())).Callback((Player player) => {
            player.Id = -1;
        });

        // Act & Assert
        Assert.Multiple(
            () => Assert.Throws<ArgumentException>(() => _service.AddMembersAsTeam(poule.Id.Value, [member1.Id.Value, member2.Id.Value, member3.Id.Value, member4.Id.Value, -5])),
            () => _mockPouleRepository.Verify(repo => repo.GetWithAncestors(It.IsAny<int>()), Times.Once),
            () => _mockMemberService.Verify(member => member.Get(It.IsAny<int>()), Times.Exactly(5)),
            () => _mockPlayerService.Verify(player => player.Insert(It.IsAny<Player>()), Times.Once),
            () => _mockMemberService.Verify(member => member.Update(It.IsAny<int>(), It.IsAny<Member>()), Times.Exactly(2)),
            () => Assert.Equal(-1, member1.PlayerId),
            () => Assert.Equal(-1, member2.PlayerId)
        );
    }
}
