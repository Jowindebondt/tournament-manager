using Microsoft.AspNetCore.Mvc;
using TournamentManager.Application;
using TournamentManager.Domain;
using TournamentManager.Infrastructure;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Api.Test;

public class ComponentTestTableTennisController : IClassFixture<SimpleTableTennisDatabaseFixture>
{
    private readonly SimpleTableTennisDatabaseFixture _fixture;

    public ComponentTestTableTennisController(SimpleTableTennisDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    private TableTennisController CreateController()
    {
        return new TableTennisController(
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
                                ),
                                new PouleRepository(_fixture.DbContext)
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
                                ),
                                new PouleRepository(_fixture.DbContext)
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
            ),
            new RoundService(
                new CrudService<Round>(
                    new Repository<Round>(_fixture.DbContext)
                ),
                new CrudService<RoundSettings>(
                    new Repository<RoundSettings>(_fixture.DbContext)
                )
            )
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public void SetTournamentSettings_ReturnsOkResult()
    {
        // arrange
        var settings = new TableTennisSettings{
            TournamentId = -1,
            Handicap = TableTennisHandicap.None,
            TournamentType = TableTennisTournamentType.Single
        };

        // act
        var result = CreateController().SetTournamentSettings(settings);

        // assert
        Assert.Multiple(
            () => Assert.IsType<OkResult>(result)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public void SetRoundSettings_ReturnsOkResult()
    {
        // arrange
        var settings = new TableTennisRoundSettings{
            RoundId = -1,
            BestOf = 5,
        };

        // act
        var result = CreateController().SetRoundSettings(settings);

        // assert
        Assert.Multiple(
            () => Assert.IsType<OkResult>(result)
        );
    }
}
