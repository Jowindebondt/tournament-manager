using Microsoft.AspNetCore.Mvc;
using TournamentManager.Application;
using TournamentManager.Domain;
using TournamentManager.Infrastructure;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Api.Test;

public class ComponentTestMatchController : IClassFixture<SimpleMatchDatabaseFixture>
{
    private readonly SimpleMatchDatabaseFixture _fixture;

    public ComponentTestMatchController(SimpleMatchDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    private MatchController CreateController()
    {
        return new MatchController(
            new MatchService(
                new PouleService(
                    new RoundService(
                        new TournamentService(
                            new CrudService<Tournament>(
                                new Repository<Tournament>(_fixture.DbContext)
                            )
                        ),
                        new CrudService<Round>(
                            new Repository<Round>(_fixture.DbContext)
                        )
                    ),
                    new CrudService<Poule>(
                        new Repository<Poule>(_fixture.DbContext)
                    )
                ),
                new CrudService<Match>(
                    new Repository<Match>(_fixture.DbContext)
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
        IEnumerable<Match> matches = null;

        // act
        var result = CreateController().GetList(-1);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => matches = Assert.IsAssignableFrom<IEnumerable<Match>>(okResult.Value),
            () => Assert.Equal(_fixture.DbContext.Matches.Count(), matches.Count())
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
        Match match = null;

        // act
        var result = CreateController().GetById(id);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => match = Assert.IsType<Match>(okResult.Value),
            () => Assert.Equal(id, match.Id)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public void CreateValidInstance_ReturnsOkWithEntity()
    {
        // arrange
        var newInstance = new Match()
        {
            Player_1 = _fixture.DbContext.Members.Find(-1),
            Player_2 = _fixture.DbContext.Members.Find(-2),
        };
        OkObjectResult okResult = null;
        Match addedInstance = null;

        // act
        var result = CreateController().Create(-1, newInstance);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => addedInstance = Assert.IsType<Match>(okResult.Value),
            () => Assert.NotNull(addedInstance.Id),
            () => Assert.NotNull(addedInstance.Poule),
            () => Assert.NotNull(addedInstance.Player_1),
            () => Assert.NotNull(addedInstance.Player_2),
            () => Assert.NotNull(addedInstance.CreatedDate),
            () => Assert.NotNull(addedInstance.ModifiedDate)
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
