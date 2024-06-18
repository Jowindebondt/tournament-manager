using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TournamentManager.Application;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using TournamentManager.Infrastructure;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Api.Test.TournamentApi;

public class ComponentTestTournamentController : IClassFixture<SimpleTournamentDatabaseFixture>
{
    private readonly SimpleTournamentDatabaseFixture _fixture;

    public ComponentTestTournamentController(SimpleTournamentDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    private TournamentController CreateController()
    {
        return new TournamentController(
            new TournamentService(
                new CrudService<Tournament>(
                    new Repository<Tournament>(_fixture.DbContext)
                ),
                new TournamentRepository(_fixture.DbContext),
                new CrudService<TournamentSettings>(
                    new Repository<TournamentSettings>(_fixture.DbContext)
                ),
                delegate(Sport key) {
                    return new TableTennisService(
                        new TableTennisRoundGenerator(
                            new RoundService(
                                new CrudService<Round>(
                                    new Repository<Round>(_fixture.DbContext)
                                ),
                                new CrudService<RoundSettings>(
                                    new Repository<RoundSettings>(_fixture.DbContext)
                                )
                            )
                        ),
                        new TableTennisPouleGenerator(
                            delegate(int key) {
                                return new Poule3PlayerTemplateService();
                            },
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
                        ),
                        new TableTennisMatchGenerator(
                            delegate(int key) {
                                return new Poule3PlayerTemplateService();
                            },
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
                        ),
                        new TableTennisGameGenerator(
                            new GameService(
                                new CrudService<Game>(
                                    new Repository<Game>(_fixture.DbContext)
                                )
                            )
                        )
                    );
                }
            )
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public void GetList_ReturnsOkWithFilledList()
    {
        // arrange
        OkObjectResult okResult = null;
        IEnumerable<Tournament> tournaments = null;

        // act
        var result = CreateController().GetList();

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => tournaments = Assert.IsAssignableFrom<IEnumerable<Tournament>>(okResult.Value),
            () => Assert.Equal(_fixture.DbContext.Tournaments.Count(), tournaments.Count())
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
        Tournament tournament = null;

        // act
        var result = CreateController().GetById(id);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => tournament = Assert.IsType<Tournament>(okResult.Value),
            () => Assert.Equal(id, tournament.Id)
        );
    }

    [Theory]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    [InlineData("Test_Tournament_New_1")]
    [InlineData("Test_Tournament_New_2")]
    public void CreateValidInstance_ReturnsOkWithEntity(string name)
    {
        // arrange
        var newInstance = new Tournament()
        {
            Name = name,
        };
        OkObjectResult okResult = null;
        Tournament addedInstance = null;

        // act
        var result = CreateController().Create(newInstance);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => addedInstance = Assert.IsType<Tournament>(okResult.Value),
            () => Assert.NotNull(addedInstance.Id),
            () => Assert.NotNull(addedInstance.CreatedDate),
            () => Assert.NotNull(addedInstance.ModifiedDate)
        );
    }

    [Theory]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    [InlineData(-1, "Updating_Test_Tournament_1")]
    [InlineData(-2, "Updating_Test_Tournament_2")]
    public void UpdateValidInstance_ReturnsOkWithEntity(int id, string newName)
    {
        // arrange
        var updatingInstance = new Tournament() 
        {
            Name = newName
        };
        OkObjectResult okResult = null;
        Tournament updatedInstance = null;

        // act
        var result = CreateController().Update(id, updatingInstance);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => updatedInstance = Assert.IsType<Tournament>(okResult.Value),
            () => Assert.Equal(id, updatedInstance.Id),
            () => Assert.Equal(updatingInstance.Name, updatedInstance.Name),
            () => Assert.NotEqual(updatedInstance.CreatedDate, updatedInstance.ModifiedDate)
        );
    }

    [Theory]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    [InlineData(-3)]
    [InlineData(-4)]
    public void Delete_ReturnsOkResult(int id)
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
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public void Generate_ReturnsOkResult()
    {
        // arrange

        // act
        var result = CreateController().Generate(-1);

        // assert
        Assert.Multiple(
            () => Assert.IsType<OkResult>(result),
            () => Assert.Equal(1, _fixture.DbContext.Rounds.Where(u => u.TournamentId == -1).Count()),
            () => Assert.Equal(1, _fixture.DbContext.Poules.Where(u => u.Round.TournamentId == -1).Count()),
            () => Assert.Equal(3, _fixture.DbContext.Matches.Where(u => u.Poule.Round.TournamentId == -1).Count()),
            () => Assert.Equal(15, _fixture.DbContext.Games.Where(u => u.Match.Poule.Round.TournamentId == -1).Count())
        );
    }
}
