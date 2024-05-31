using Microsoft.AspNetCore.Mvc;
using TournamentManager.Application;
using TournamentManager.Domain;
using TournamentManager.Infrastructure;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Api.Test;

public class ComponentTestMemberController : IClassFixture<SimpleMemberDatabaseFixture>
{
    private readonly SimpleMemberDatabaseFixture _fixture;

    public ComponentTestMemberController(SimpleMemberDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    private MemberController CreateController()
    {
        return new MemberController(
            new MemberService(
                new CrudService<Member>(
                    new Repository<Member>(_fixture.DbContext)
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
        IEnumerable<Member> members = null;

        // act
        var result = CreateController().GetList(-1);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => members = Assert.IsAssignableFrom<IEnumerable<Member>>(okResult.Value),
            () => Assert.Equal(_fixture.DbContext.Members.Count(), members.Count())
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
        Member round = null;

        // act
        var result = CreateController().GetById(id);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => round = Assert.IsType<Member>(okResult.Value),
            () => Assert.Equal(id, round.Id)
        );
    }

    [Theory]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    [InlineData("Test_Member_New_1")]
    [InlineData("Test_Member_New_2")]
    public void CreateValidInstance_ReturnsOkWithEntity(string name)
    {
        // arrange
        var newInstance = new Member()
        {
            Name = name,
            TournamentId = -1,
        };
        OkObjectResult okResult = null;
        Member addedInstance = null;

        // act
        var result = CreateController().Create(newInstance);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => addedInstance = Assert.IsType<Member>(okResult.Value),
            () => Assert.NotNull(addedInstance.Id),
            () => Assert.NotNull(addedInstance.TournamentId),
            () => Assert.NotNull(addedInstance.CreatedDate),
            () => Assert.NotNull(addedInstance.ModifiedDate)
        );
    }

    [Theory]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    [InlineData(-1, "Updating_Test_Member_1")]
    [InlineData(-2, "Updating_Test_Member_2")]
    public void UpdateValidInstance_ReturnsOkWithEntity(int id, string newName)
    {
        // arrange
        var updatingInstance = new Member()
        {
            Name = newName
        };
        OkObjectResult okResult = null;
        Member updatedInstance = null;

        // act
        var result = CreateController().Update(id, updatingInstance);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => updatedInstance = Assert.IsType<Member>(okResult.Value),
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
}
