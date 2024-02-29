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
            )
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public void GetList_ReturnsOkWithFilledList()
    {
        // arrange
        OkObjectResult okResult = null;
        IEnumerable<Poule> rounds = null;

        // act
        var result = CreateController().GetList(-1);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => rounds = Assert.IsAssignableFrom<IEnumerable<Poule>>(okResult.Value),
            () => Assert.Equal(_fixture.DbContext.Poules.Count(), rounds.Count())
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
        Poule round = null;

        // act
        var result = CreateController().GetById(id);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => round = Assert.IsType<Poule>(okResult.Value),
            () => Assert.Equal(id, round.Id)
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
        };
        OkObjectResult okResult = null;
        Poule addedInstance = null;

        // act
        var result = CreateController().Create(-1, newInstance);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => addedInstance = Assert.IsType<Poule>(okResult.Value),
            () => Assert.NotNull(addedInstance.Id),
            () => Assert.NotNull(addedInstance.Round),
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
}
