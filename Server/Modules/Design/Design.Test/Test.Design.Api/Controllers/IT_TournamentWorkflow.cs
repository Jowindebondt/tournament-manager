using AutoMapper;
using Competition.Application.Interfaces;
using Competition.Application.Services;
using Competition.Domain.Interfaces;
using Competition.Infrastructure.Repositories;
using Design.Application.Interfaces;
using Design.Application.Services;
using Design.Application.Tournaments.Commands;
using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using Design.Infrastructure.Repositories;
using Generation.Application.Tournaments.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sports.TableTennis.Api.ViewModels;
using Test.Design.Api.Fixtures;
using TournamentManager.TestHelper;
using Xunit;
using Design.Api.ViewModels;

namespace Test.Design.Api.Controllers;

public class IT_TournamentWorkflow : IDisposable
{
    private readonly TestDesignDbContext _dbContext;
    private readonly TestCompetitionDbContext _competitionDbContext;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public IT_TournamentWorkflow()
    {
        var options = new DbContextOptionsBuilder<TestDesignDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _dbContext = new TestDesignDbContext(options);

        var competitionOptions = new DbContextOptionsBuilder<TestCompetitionDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _competitionDbContext = new TestCompetitionDbContext(competitionOptions);

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(
            typeof(global::Design.Api.MappingProfile),
            typeof(global::Design.Application.MappingProfile));
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateTournamentCommand).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(GenerateCompetitionCommand).Assembly);
        });
        services.AddSingleton<ITournamentRepository>(new TournamentRepository(_dbContext));
        services.AddSingleton<IRoundRepository>(new RoundRepository(_dbContext));
        services.AddSingleton<IPouleRepository>(new PouleRepository(_dbContext));
        services.AddSingleton<ICompetitionRepository>(new CompetitionRepository(_competitionDbContext));
        services.AddScoped<IDesignModuleApi, DesignModuleService>();
        services.AddScoped<ICompetitionModuleApi, CompetitionModuleService>();
        var sp = services.BuildServiceProvider();
        _mapper = sp.GetRequiredService<IMapper>();
        _mediator = sp.GetRequiredService<IMediator>();
    }

    private global::Design.Api.Controllers.TournamentController CreateTournamentController()
        => new(_mapper, _mediator);

    private global::Design.Api.Controllers.RoundController CreateRoundController()
        => new(_mapper, _mediator);

    private global::Design.Api.Controllers.PouleController CreatePouleController()
        => new(_mapper, _mediator);

    private global::Generation.Api.Controllers.GenerationController CreateGenerationController()
        => new(_mediator);

    private global::Sports.TableTennis.Api.Controllers.TableTennisRoundController CreateTableTennisRoundController()
        => new(_mediator);

    public void Dispose()
    {
        _dbContext.Dispose();
        _competitionDbContext.Dispose();
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.IntegrationTest)]
    public async Task CreateTournamentWithSingleRoundAndTwoPoules_AllEntitiesPersistedCorrectly()
    {
        // act - step 1: create tournament
        var tournamentResult = await CreateTournamentController().CreateAsync(
            new CreateTournamentViewModel { Name = "Test Tournament", Sport = "TableTennis" });
        var tournament = (TournamentViewModel)((CreatedAtActionResult)tournamentResult).Value!;

        // act - step 2: create round
        var roundResult = await CreateRoundController().CreateAsync(
            new CreateRoundViewModel { Name = "Round 1", TournamentId = tournament.Id });
        var round = (RoundViewModel)((CreatedAtActionResult)roundResult).Value!;

        // act - step 3: create two poules
        var poule1Result = await CreatePouleController().CreateAsync(
            new CreatePouleViewModel { Name = "Poule A", TotalPlayers = 4, RoundId = round.Id });
        var poule2Result = await CreatePouleController().CreateAsync(
            new CreatePouleViewModel { Name = "Poule B", TotalPlayers = 4, RoundId = round.Id });

        // assert
        Assert.Multiple(
            () => Assert.IsType<CreatedAtActionResult>(tournamentResult),
            () => Assert.IsType<CreatedAtActionResult>(roundResult),
            () => Assert.IsType<CreatedAtActionResult>(poule1Result),
            () => Assert.IsType<CreatedAtActionResult>(poule2Result),
            () => Assert.Equal(1, _dbContext.Tournaments.Count()),
            () => Assert.Equal(1, _dbContext.Rounds.Count()),
            () => Assert.Equal(2, _dbContext.Poules.Count())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.IntegrationTest)]
    public async Task CreateTournamentWithTwoRoundsEachWithTwoPoules_Round2ReferencesPositionsFromRound1_AllEntitiesPersistedCorrectly()
    {
        // act - step 1: create tournament
        var tournamentResult = await CreateTournamentController().CreateAsync(
            new CreateTournamentViewModel { Name = "Multi-Round Tournament", Sport = "TableTennis" });
        var tournament = (TournamentViewModel)((CreatedAtActionResult)tournamentResult).Value!;

        // act - step 2: create round 1 and its 2 poules
        var round1Result = await CreateRoundController().CreateAsync(
            new CreateRoundViewModel { Name = "Round 1", TournamentId = tournament.Id });
        var round1 = (RoundViewModel)((CreatedAtActionResult)round1Result).Value!;

        var poule1AResult = await CreatePouleController().CreateAsync(
            new CreatePouleViewModel { Name = "Poule 1A", TotalPlayers = 4, RoundId = round1.Id });
        var poule1A = (PouleViewModel)((CreatedAtActionResult)poule1AResult).Value!;

        var poule1BResult = await CreatePouleController().CreateAsync(
            new CreatePouleViewModel { Name = "Poule 1B", TotalPlayers = 4, RoundId = round1.Id });
        var poule1B = (PouleViewModel)((CreatedAtActionResult)poule1BResult).Value!;

        // act - step 3: create round 2 and configure it as the successor of round 1
        var round2Result = await CreateRoundController().CreateAsync(
            new CreateRoundViewModel { Name = "Round 2", TournamentId = tournament.Id });
        var round2 = (RoundViewModel)((CreatedAtActionResult)round2Result).Value!;

        // settings are required before configuring poule position mappings
        // Note: SetTableTennisSettings lives in Sports.TableTennis.Api (correct module boundary)
        var setSettingsResult = await CreateTableTennisRoundController().SetTableTennisSettingsAsync(
            round2.Id,
            new SetTableTennisSettingsRoundViewModel { BestOf = 5 });

        var setPreviousResult = await CreateRoundController().SetPreviousRoundAsync(
            round2.Id,
            new SetPreviousRoundViewModel { PreviousId = round1.Id });

        // act - step 4: create 2 poules in round 2
        var poule2AResult = await CreatePouleController().CreateAsync(
            new CreatePouleViewModel { Name = "Poule 2A", TotalPlayers = 4, RoundId = round2.Id });
        var poule2A = (PouleViewModel)((CreatedAtActionResult)poule2AResult).Value!;

        var poule2BResult = await CreatePouleController().CreateAsync(
            new CreatePouleViewModel { Name = "Poule 2B", TotalPlayers = 4, RoundId = round2.Id });

        // act - step 5: configure round 2 poule 2A to accept:
        //   - 1st place from round 1 poule 1A
        //   - 1st place from round 1 poule 1B
        var setPositionsResult = await CreateRoundController().SetRoundPoulePositions(
            round2.Id,
            [
                new SetRoundPoulePositionViewModel
                {
                    PreviousPouleId = poule1A.Id,
                    PreviousPosition = 1,
                    CurrentPouleId = poule2A.Id,
                    CurrentPosition = 1
                },
                new SetRoundPoulePositionViewModel
                {
                    PreviousPouleId = poule1B.Id,
                    PreviousPosition = 1,
                    CurrentPouleId = poule2A.Id,
                    CurrentPosition = 2
                }
            ]);

        // assert
        Assert.Multiple(
            () => Assert.IsType<NoContentResult>(setSettingsResult),
            () => Assert.IsType<NoContentResult>(setPreviousResult),
            () => Assert.IsType<NoContentResult>(setPositionsResult),
            () => Assert.Equal(1, _dbContext.Tournaments.Count()),
            () => Assert.Equal(2, _dbContext.Rounds.Count()),
            () => Assert.Equal(4, _dbContext.Poules.Count()),
            () => Assert.Equal(2, _dbContext.Poules.Count(p => p.RoundId == new RoundId(round1.Id))),
            () => Assert.Equal(2, _dbContext.Poules.Count(p => p.RoundId == new RoundId(round2.Id)))
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.IntegrationTest)]
    public async Task CreateTwoTournamentsEachWithRoundsAndPoules_PoulesBelongToCorrectRoundsAndTournaments()
    {
        // act - create tournament A with 1 round and 2 poules
        var tournamentAResult = await CreateTournamentController().CreateAsync(
            new CreateTournamentViewModel { Name = "Tournament A", Sport = "TableTennis" });
        var tournamentA = (TournamentViewModel)((CreatedAtActionResult)tournamentAResult).Value!;

        var roundAResult = await CreateRoundController().CreateAsync(
            new CreateRoundViewModel { Name = "Round A1", TournamentId = tournamentA.Id });
        var roundA = (RoundViewModel)((CreatedAtActionResult)roundAResult).Value!;

        await CreatePouleController().CreateAsync(
            new CreatePouleViewModel { Name = "Poule A1", TotalPlayers = 4, RoundId = roundA.Id });
        await CreatePouleController().CreateAsync(
            new CreatePouleViewModel { Name = "Poule A2", TotalPlayers = 4, RoundId = roundA.Id });

        // act - create tournament B with 2 rounds and 1 poule per round
        var tournamentBResult = await CreateTournamentController().CreateAsync(
            new CreateTournamentViewModel { Name = "Tournament B", Sport = "TableTennis" });
        var tournamentB = (TournamentViewModel)((CreatedAtActionResult)tournamentBResult).Value!;

        var roundB1Result = await CreateRoundController().CreateAsync(
            new CreateRoundViewModel { Name = "Round B1", TournamentId = tournamentB.Id });
        var roundB1 = (RoundViewModel)((CreatedAtActionResult)roundB1Result).Value!;

        var roundB2Result = await CreateRoundController().CreateAsync(
            new CreateRoundViewModel { Name = "Round B2", TournamentId = tournamentB.Id });
        var roundB2 = (RoundViewModel)((CreatedAtActionResult)roundB2Result).Value!;

        await CreatePouleController().CreateAsync(
            new CreatePouleViewModel { Name = "Poule B1", TotalPlayers = 4, RoundId = roundB1.Id });
        await CreatePouleController().CreateAsync(
            new CreatePouleViewModel { Name = "Poule B2", TotalPlayers = 4, RoundId = roundB2.Id });

        // assert total counts
        Assert.Multiple(
            () => Assert.Equal(2, _dbContext.Tournaments.Count()),
            () => Assert.Equal(3, _dbContext.Rounds.Count()),
            () => Assert.Equal(4, _dbContext.Poules.Count()),
            // tournament A: 1 round, 2 poules in that round
            () => Assert.Equal(2, _dbContext.Poules.Count(p => p.RoundId == new RoundId(roundA.Id))),
            // tournament B: 2 rounds, 1 poule each
            () => Assert.Equal(1, _dbContext.Poules.Count(p => p.RoundId == new RoundId(roundB1.Id))),
            () => Assert.Equal(1, _dbContext.Poules.Count(p => p.RoundId == new RoundId(roundB2.Id)))
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.IntegrationTest)]
    public async Task GetRoundsByTournament_AfterCreatingRoundsForMultipleTournaments_ReturnsOnlyRoundsForRequestedTournament()
    {
        // act - create two tournaments each with rounds
        var tournamentAResult = await CreateTournamentController().CreateAsync(
            new CreateTournamentViewModel { Name = "Tournament A", Sport = "TableTennis" });
        var tournamentA = (TournamentViewModel)((CreatedAtActionResult)tournamentAResult).Value!;

        await CreateRoundController().CreateAsync(new CreateRoundViewModel { Name = "Round A1", TournamentId = tournamentA.Id });
        await CreateRoundController().CreateAsync(new CreateRoundViewModel { Name = "Round A2", TournamentId = tournamentA.Id });

        var tournamentBResult = await CreateTournamentController().CreateAsync(
            new CreateTournamentViewModel { Name = "Tournament B", Sport = "TableTennis" });
        var tournamentB = (TournamentViewModel)((CreatedAtActionResult)tournamentBResult).Value!;

        await CreateRoundController().CreateAsync(new CreateRoundViewModel { Name = "Round B1", TournamentId = tournamentB.Id });

        // act - retrieve rounds per tournament
        var roundsForAResult = await CreateRoundController().GetAllByTournamentAsync(tournamentA.Id);
        var roundsForBResult = await CreateRoundController().GetAllByTournamentAsync(tournamentB.Id);

        var roundsForA = (IEnumerable<RoundViewModel>)((OkObjectResult)roundsForAResult).Value!;
        var roundsForB = (IEnumerable<RoundViewModel>)((OkObjectResult)roundsForBResult).Value!;

        // assert
        Assert.Multiple(
            () => Assert.Equal(2, roundsForA.Count()),
            () => Assert.Single(roundsForB),
            () => Assert.All(roundsForA, r => Assert.Equal(tournamentA.Id, r.Tournament.Id)),
            () => Assert.All(roundsForB, r => Assert.Equal(tournamentB.Id, r.Tournament.Id))
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.IntegrationTest)]
    public async Task GenerateTournamentWithRoundsAndPoules_CompetitionCreatedWithMatchingStructure()
    {
        // act - step 1: create tournament with 1 round and 2 poules
        var tournamentResult = await CreateTournamentController().CreateAsync(
            new CreateTournamentViewModel { Name = "Generated Tournament", Sport = "TableTennis" });
        var tournament = (TournamentViewModel)((CreatedAtActionResult)tournamentResult).Value!;

        var roundResult = await CreateRoundController().CreateAsync(
            new CreateRoundViewModel { Name = "Round 1", TournamentId = tournament.Id });
        var round = (RoundViewModel)((CreatedAtActionResult)roundResult).Value!;

        await CreatePouleController().CreateAsync(
            new CreatePouleViewModel { Name = "Poule A", TotalPlayers = 4, RoundId = round.Id });
        await CreatePouleController().CreateAsync(
            new CreatePouleViewModel { Name = "Poule B", TotalPlayers = 4, RoundId = round.Id });

        // act - step 2: generate competition from tournament
        var generateResult = await CreateGenerationController().GenerateAsync(tournament.Id);

        // assert
        Assert.Multiple(
            () => Assert.IsType<NoContentResult>(generateResult),
            () => Assert.Equal(1, _competitionDbContext.Competitions.Count()),
            () => Assert.Equal(1, _competitionDbContext.Rounds.Count()),
            () => Assert.Equal(2, _competitionDbContext.Poules.Count()),
            () => Assert.Equal("Generated Tournament", _competitionDbContext.Competitions.First().Name.Value)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.IntegrationTest)]
    public async Task GenerateTournamentTwice_TwoIndependentCompetitionsCreated()
    {
        // act - step 1: create tournament
        var tournamentResult = await CreateTournamentController().CreateAsync(
            new CreateTournamentViewModel { Name = "Reusable Design", Sport = "TableTennis" });
        var tournament = (TournamentViewModel)((CreatedAtActionResult)tournamentResult).Value!;

        var roundResult = await CreateRoundController().CreateAsync(
            new CreateRoundViewModel { Name = "Round 1", TournamentId = tournament.Id });
        var round = (RoundViewModel)((CreatedAtActionResult)roundResult).Value!;

        await CreatePouleController().CreateAsync(
            new CreatePouleViewModel { Name = "Poule A", TotalPlayers = 3, RoundId = round.Id });

        // act - step 2: generate competition twice from same design
        await CreateGenerationController().GenerateAsync(tournament.Id);
        await CreateGenerationController().GenerateAsync(tournament.Id);

        // assert: two independent competitions are created from the same design
        Assert.Multiple(
            () => Assert.Equal(2, _competitionDbContext.Competitions.Count()),
            () => Assert.Equal(2, _competitionDbContext.Rounds.Count()),
            () => Assert.Equal(2, _competitionDbContext.Poules.Count()),
            () => Assert.All(_competitionDbContext.Competitions, c => Assert.Equal("Reusable Design", c.Name.Value))
        );
    }
}
