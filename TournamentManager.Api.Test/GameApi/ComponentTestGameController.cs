using Microsoft.AspNetCore.Mvc;
using TournamentManager.Application;
using TournamentManager.Domain;
using TournamentManager.Infrastructure;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Api.Test;

public class ComponentTestGameController: IClassFixture<SimpleGameDatabaseFixture>
{
    private readonly SimpleGameDatabaseFixture _fixture;

    public ComponentTestGameController(SimpleGameDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    private GameController CreateController()
    {
        return new GameController(
            new GameService(
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
                ),
                new CrudService<Game>(
                    new Repository<Game>(_fixture.DbContext)
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
        IEnumerable<Game> games = null;

        // act
        var result = CreateController().GetList(-1);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => games = Assert.IsAssignableFrom<IEnumerable<Game>>(okResult.Value),
            () => Assert.Equal(_fixture.DbContext.Games.Count(), games.Count())
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
        Game game = null;

        // act
        var result = CreateController().GetById(id);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => game = Assert.IsType<Game>(okResult.Value),
            () => Assert.Equal(id, game.Id)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public void CreateValidInstance_ReturnsOkWithEntity()
    {
        // arrange
        var newInstance = new Game()
        {
            Score_1 = 0,
            Score_2 = 0,
        };
        OkObjectResult okResult = null;
        Game addedInstance = null;

        // act
        var result = CreateController().Create(-1, newInstance);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => addedInstance = Assert.IsType<Game>(okResult.Value),
            () => Assert.NotNull(addedInstance.Id),
            () => Assert.NotNull(addedInstance.Match),
            () => Assert.NotNull(addedInstance.CreatedDate),
            () => Assert.NotNull(addedInstance.ModifiedDate)
        );
    }

    [Theory]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    [InlineData(-1, 11, 9)]
    [InlineData(-2, 9, 11)]
    public void UpdateValidInstance_ReturnsOkWithEntity(int id, int score_1, int score_2)
    {
        // arrange
        var updatingInstance = new Game()
        {
            Score_1 = score_1,
            Score_2 = score_2,
        };

        OkObjectResult okResult = null;
        Game updatedInstance = null;

        // act
        var result = CreateController().Update(id, updatingInstance);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => updatedInstance = Assert.IsType<Game>(okResult.Value),
            () => Assert.Equal(id, updatedInstance.Id),
            () => Assert.Equal(updatingInstance.Score_1, updatedInstance.Score_1),
            () => Assert.Equal(updatingInstance.Score_2, updatedInstance.Score_2),
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
