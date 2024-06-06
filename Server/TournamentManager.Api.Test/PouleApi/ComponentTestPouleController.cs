using Microsoft.AspNetCore.Mvc;
using TournamentManager.Application;
using TournamentManager.Domain;
using TournamentManager.Infrastructure;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Api.Test;

public class ComponentTestPouleController : IClassFixture<SimplePouleDatabaseFixture>
{
    private readonly SimplePouleDatabaseFixture _fixture;

    public ComponentTestPouleController(SimplePouleDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    private PouleController CreateController()
    {
        return new PouleController(
            new PouleService(
                new CrudService<Poule>(
                    new Repository<Poule>(_fixture.DbContext)
                ),
                new MemberService(
                    new CrudService<Member>(
                        new Repository<Member>(_fixture.DbContext)
                    )
                ),
                new PlayerService(
                    new CrudService<Player>(
                        new Repository<Player>(_fixture.DbContext)
                    )
                )
            )
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public void GetList_ReturnsOkWithFilledList()
    {
        // arrange
        OkObjectResult okResult = null;
        IEnumerable<Poule> poules = null;

        // act
        var result = CreateController().GetList(-1);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => poules = Assert.IsAssignableFrom<IEnumerable<Poule>>(okResult.Value),
            () => Assert.Equal(_fixture.DbContext.Poules.Count(), poules.Count())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public void GetById_ReturnsNotFound()
    {
        // arrange

        // act
        var result = CreateController().GetById(0);

        // assert
        Assert.Multiple(
            () => Assert.IsType<NotFoundResult>(result)
        );
    }

    [Theory]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    [InlineData(-1)]
    [InlineData(-5)]
    public void GetById_ReturnsOkWithEntity_DBSetFindCalledOnce(int id)
    {
        // arrange
        OkObjectResult okResult = null;
        Poule poule = null;

        // act
        var result = CreateController().GetById(id);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => poule = Assert.IsType<Poule>(okResult.Value),
            () => Assert.Equal(id, poule.Id)
        );
    }

    [Theory]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    [InlineData("Test_Poule_New_1")]
    [InlineData("Test_Poule_New_2")]
    public void CreateValidInstance_ReturnsOkWithEntity(string name)
    {
        // arrange
        var newInstance = new Poule()
        {
            Name = name,
            RoundId = -1,
        };
        OkObjectResult okResult = null;
        Poule addedInstance = null;

        // act
        var result = CreateController().Create(newInstance);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => addedInstance = Assert.IsType<Poule>(okResult.Value),
            () => Assert.NotNull(addedInstance.Id),
            () => Assert.NotNull(addedInstance.RoundId),
            () => Assert.NotNull(addedInstance.CreatedDate),
            () => Assert.NotNull(addedInstance.ModifiedDate)
        );
    }

    [Theory]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    [InlineData(-1, "Updating_Test_Poule_1")]
    [InlineData(-2, "Updating_Test_Poule_2")]
    public void UpdateValidInstance_ReturnsOkWithEntity(int id, string newName)
    {
        // arrange
        var updatingInstance = new Poule()
        {
            Name = newName
        };
        OkObjectResult okResult = null;
        Poule updatedInstance = null;

        // act
        var result = CreateController().Update(id, updatingInstance);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => updatedInstance = Assert.IsType<Poule>(okResult.Value),
            () => Assert.Equal(id, updatedInstance.Id),
            () => Assert.Equal(updatingInstance.Name, updatedInstance.Name),
            () => Assert.NotEqual(updatedInstance.CreatedDate, updatedInstance.ModifiedDate)
        );
    }

    [Theory]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    [InlineData(-3)]
    [InlineData(-4)]
    public void Delete(int id)
    {
        // arrange

        // act
        var result = CreateController().Delete(id);

        // assert
        Assert.Multiple(
            () => Assert.IsType<OkResult>(result)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void AddValidMembers_ReturnsOk()
    {
        // arrange

        // act
        var result = CreateController().AddMembers(-1, [-1,-2,-3]);

        // assert
        Assert.Multiple(
            () => Assert.IsType<OkResult>(result)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void AddNonExistingMembers_ThrowsArgumentException()
    {
        // arrange

        // act & assert
        Assert.Multiple(
            () => Assert.Throws<ArgumentException>(() => CreateController().AddMembers(-1, [-10,-20,-30]))
        );
    }
    
    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void AddValidMembersAsTeam_ReturnsOk()
    {
        // arrange

        // act
        var result = CreateController().AddMembersAsTeam(-1, [-1,-2,-3]);

        // assert
        Assert.Multiple(
            () => Assert.IsType<OkResult>(result)
        );
    }
    
    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void AddNonExistingMembersAsTeam_ThrowsArgumentException()
    {
        // arrange

        // act
        Assert.Multiple(
            () => Assert.Throws<ArgumentException>(() => CreateController().AddMembersAsTeam(-1, [-10,-20,-30]))
        );
    }
}
