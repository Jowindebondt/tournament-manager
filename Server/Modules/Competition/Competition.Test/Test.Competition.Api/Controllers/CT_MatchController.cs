using AutoMapper;
using Competition.Application.Matches.Commands;
using Competition.Domain.Entities;
using Competition.Domain.ValueObjects;
using Competition.Infrastructure.Repositories;
using Competition.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Test.Competition.Api.Fixtures;
using TournamentManager.TestHelper;
using Xunit;
using Competition.Api.ViewModels;
using CompetitionEntity = Competition.Domain.Entities.Competition;

namespace Test.Competition.Api.Controllers;

public class CT_MatchController : IDisposable
{
    private readonly TestCompetitionDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    private readonly Guid _competitionId;
    private readonly Guid _roundId;
    private readonly Guid _pouleId;
    private readonly Guid _existingMatchId;
    private readonly Guid _saveResultMatchId;
    private readonly int _seedMatchCount;

    public CT_MatchController()
    {
        var options = new DbContextOptionsBuilder<TestCompetitionDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _dbContext = new TestCompetitionDbContext(options);

        _competitionId = Guid.NewGuid();
        _roundId = Guid.NewGuid();
        _pouleId = Guid.NewGuid();
        _existingMatchId = Guid.NewGuid();
        _saveResultMatchId = Guid.NewGuid();
        _seedMatchCount = 2;

        var competition = new CompetitionEntity(
            new CompetitionId(_competitionId),
            CompetitionName.Create("Test Competition"),
            global::Competition.Domain.Enums.Sport.TableTennis);
        _dbContext.Competitions.Add(competition);

        var round = new Round(
            new RoundId(_roundId),
            RoundName.Create("Round A"),
            new CompetitionId(_competitionId));
        _dbContext.Rounds.Add(round);

        var poule = new Poule(
            new PouleId(_pouleId),
            PouleName.Create("Poule A"),
            PoulePlayersCount.Create(4),
            new RoundId(_roundId));
        _dbContext.Poules.Add(poule);

        _dbContext.Matches.AddRange(
            new Match(new MatchId(_existingMatchId), 1, 2, new PouleId(_pouleId)),
            new Match(new MatchId(_saveResultMatchId), 3, 4, new PouleId(_pouleId))
        );
        _dbContext.SaveChanges();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(typeof(global::Competition.Api.MappingProfile), typeof(global::Competition.Application.MappingProfile));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SaveMatchResultCommand).Assembly));
        services.AddSingleton<IMatchRepository>(new MatchRepository(_dbContext));
        var sp = services.BuildServiceProvider();
        _mapper = sp.GetRequiredService<IMapper>();
        _mediator = sp.GetRequiredService<IMediator>();
    }

    private global::Competition.Api.Controllers.MatchController CreateController()
        => new global::Competition.Api.Controllers.MatchController(_mapper, _mediator);

    public void Dispose() => _dbContext.Dispose();

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public async Task GetAllByPouleAsync_ReturnsOk_WithFilledList()
    {
        // arrange
        OkObjectResult okResult = null!;
        IEnumerable<MatchViewModel> content = null!;

        // act
        var result = await CreateController().GetAllByPouleAsync(_pouleId);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => content = Assert.IsAssignableFrom<IEnumerable<MatchViewModel>>(okResult.Value),
            () => Assert.Equal(_seedMatchCount, content.Count())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public async Task GetByIdAsync_ExistingId_ReturnsOk()
    {
        // arrange
        OkObjectResult okResult = null!;
        MatchViewModel viewModel = null!;

        // act
        var result = await CreateController().GetByIdAsync(_existingMatchId);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => viewModel = Assert.IsType<MatchViewModel>(okResult.Value),
            () => Assert.Equal(_existingMatchId, viewModel.Id)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public async Task GetByIdAsync_NonExistingId_ReturnsNotFound()
    {
        // arrange
        var nonExistingId = Guid.NewGuid();

        // act
        var result = await CreateController().GetByIdAsync(nonExistingId);

        // assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public async Task SaveResultAsync_ValidScores_ReturnsNoContent_ResultSavedInDb()
    {
        // arrange
        var saveResultViewModel = new SaveMatchResultViewModel { Player1Score = 3, Player2Score = 1 };

        // act
        var result = await CreateController().SaveResultAsync(_saveResultMatchId, saveResultViewModel);

        // assert
        var match = _dbContext.Matches.Single(m => m.Id == new MatchId(_saveResultMatchId));
        Assert.Multiple(
            () => Assert.IsType<NoContentResult>(result),
            () => Assert.NotNull(match.Result),
            () => Assert.Equal(3, match.Result!.Player1Score),
            () => Assert.Equal(1, match.Result!.Player2Score)
        );
    }
}
