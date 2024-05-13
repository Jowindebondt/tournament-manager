using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TournamentManager.Application;
using TournamentManager.Domain;
using TournamentManager.Infrastructure;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Api.Test.RoundApi;

public class ComponentTestRoundController : IClassFixture<SimpleRoundDatabaseFixture>
{
    private readonly SimpleRoundDatabaseFixture _fixture;

    public ComponentTestRoundController(SimpleRoundDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    private RoundController CreateController()
    {
        return new RoundController(
            new RoundService(
                new CrudService<Round>(
                    new Repository<Round>(_fixture.DbContext)
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
        IEnumerable<Round> rounds = null;

        // act
        var result = CreateController().GetList(-1);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => rounds = Assert.IsAssignableFrom<IEnumerable<Round>>(okResult.Value),
            () => Assert.Equal(_fixture.DbContext.Rounds.Count(), rounds.Count())
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
        Round round = null;

        // act
        var result = CreateController().GetById(id);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => round = Assert.IsType<Round>(okResult.Value),
            () => Assert.Equal(id, round.Id)
        );
    }

    [Theory]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    [InlineData("Test_Round_New_1")]
    [InlineData("Test_Round_New_2")]
    public void CreateValidInstance_ReturnsOkWithEntity(string name)
    {
        // arrange
        var newInstance = new Round()
        {
            Name = name,
            TournamentId = -1
        };
        OkObjectResult okResult = null;
        Round addedInstance = null;

        // act
        var result = CreateController().Create(newInstance);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => addedInstance = Assert.IsType<Round>(okResult.Value),
            () => Assert.NotNull(addedInstance.Id),
            () => Assert.NotNull(addedInstance.TournamentId),
            () => Assert.NotNull(addedInstance.CreatedDate),
            () => Assert.NotNull(addedInstance.ModifiedDate)
        );
    }

    [Theory]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    [InlineData(-1, "Updating_Test_Round_1")]
    [InlineData(-2, "Updating_Test_Round_2")]
    public void UpdateValidInstance_ReturnsOkWithEntity(int id, string newName)
    {
        // arrange
        var updatingInstance = new Round()
        {
            Name = newName
        };
        OkObjectResult okResult = null;
        Round updatedInstance = null;

        // act
        var result = CreateController().Update(id, updatingInstance);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => updatedInstance = Assert.IsType<Round>(okResult.Value),
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
