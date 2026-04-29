using AutoMapper;
using Design.Application.Poules.Commands;
using Design.Domain.Entities;
using Design.Domain.Enums;
using Design.Domain.Interfaces;
using Design.Domain.ValueObjects;
using Design.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Test.Design.Api.Fixtures;
using TournamentManager.TestHelper;
using Xunit;
using Design.Api.ViewModels;

namespace Test.Design.Api.Controllers;

public class CT_PouleController : IDisposable
{
    private readonly TestDesignDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    private readonly Guid _tournamentId;
    private readonly Guid _roundId;
    private readonly Guid _existingPouleId;
    private readonly Guid _renamePouleId;
    private readonly Guid _setTotalPlayersPouleId;
    private readonly Guid _deletePouleId;
    private readonly int _seedPouleCount;

    public CT_PouleController()
    {
        var options = new DbContextOptionsBuilder<TestDesignDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _dbContext = new TestDesignDbContext(options);

        _tournamentId = Guid.NewGuid();
        _roundId = Guid.NewGuid();
        _existingPouleId = Guid.NewGuid();
        _renamePouleId = Guid.NewGuid();
        _setTotalPlayersPouleId = Guid.NewGuid();
        _deletePouleId = Guid.NewGuid();
        _seedPouleCount = 4;

        var tournament = new Tournament(new TournamentId(_tournamentId), TournamentName.Create("Tournament A"), Sport.TableTennis);
        _dbContext.Tournaments.Add(tournament);

        var round = new Round(new RoundId(_roundId), RoundName.Create("Round A"), new TournamentId(_tournamentId));
        _dbContext.Rounds.Add(round);

        _dbContext.Poules.AddRange(
            new Poule(new PouleId(_existingPouleId), PouleName.Create("Poule A"), PoulePlayersCount.Create(4), new RoundId(_roundId)),
            new Poule(new PouleId(_renamePouleId), PouleName.Create("Poule B"), PoulePlayersCount.Create(4), new RoundId(_roundId)),
            new Poule(new PouleId(_setTotalPlayersPouleId), PouleName.Create("Poule C"), PoulePlayersCount.Create(4), new RoundId(_roundId)),
            new Poule(new PouleId(_deletePouleId), PouleName.Create("Poule D"), PoulePlayersCount.Create(4), new RoundId(_roundId))
        );
        _dbContext.SaveChanges();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(typeof(global::Design.Api.MappingProfile), typeof(global::Design.Application.MappingProfile));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreatePouleCommand).Assembly));
        services.AddSingleton<ITournamentRepository>(new TournamentRepository(_dbContext));
        services.AddSingleton<IRoundRepository>(new RoundRepository(_dbContext));
        services.AddSingleton<IPouleRepository>(new PouleRepository(_dbContext));
        var sp = services.BuildServiceProvider();
        _mapper = sp.GetRequiredService<IMapper>();
        _mediator = sp.GetRequiredService<IMediator>();
    }

    private global::Design.Api.Controllers.PouleController CreateController()
        => new global::Design.Api.Controllers.PouleController(_mapper, _mediator);

    public void Dispose() => _dbContext.Dispose();

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public async Task GetAllByTournamentAndRoundAsync_ReturnsOk_WithFilledList()
    {
        // arrange
        OkObjectResult okResult = null!;
        IEnumerable<PouleViewModel> content = null!;

        // act
        var result = await CreateController().GetAllByTournamentAndRoundAsync(_tournamentId, _roundId);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => content = Assert.IsAssignableFrom<IEnumerable<PouleViewModel>>(okResult.Value),
            () => Assert.Equal(_seedPouleCount, content.Count())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public async Task GetByIdAsync_ExistingId_ReturnsOk()
    {
        // arrange
        OkObjectResult okResult = null!;
        PouleViewModel viewModel = null!;

        // act
        var result = await CreateController().GetByIdAsync(_existingPouleId);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => viewModel = Assert.IsType<PouleViewModel>(okResult.Value),
            () => Assert.Equal(_existingPouleId, viewModel.Id)
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
    public async Task CreateAsync_ValidInput_ReturnsCreatedAtAction_PouleAddedToDb()
    {
        // arrange
        var createViewModel = new CreatePouleViewModel { Name = "New Poule", TotalPlayers = 4, RoundId = _roundId };
        CreatedAtActionResult createdResult = null!;
        PouleViewModel viewModel = null!;

        // act
        var result = await CreateController().CreateAsync(createViewModel);

        // assert
        Assert.Multiple(
            () => createdResult = Assert.IsType<CreatedAtActionResult>(result),
            () => Assert.NotNull(createdResult.Value),
            () => viewModel = Assert.IsType<PouleViewModel>(createdResult.Value),
            () => Assert.Equal("New Poule", viewModel.Name),
            () => Assert.Equal(4, viewModel.TotalPlayers),
            () => Assert.Equal(_seedPouleCount + 1, _dbContext.Poules.Count())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public async Task RenameAsync_ReturnsNoContent_PouleRenamedInDb()
    {
        // arrange
        var renameViewModel = new RenamePouleViewModel { Name = "Renamed Poule" };

        // act
        var result = await CreateController().RenameAsync(_renamePouleId, renameViewModel);

        // assert
        Assert.Multiple(
            () => Assert.IsType<NoContentResult>(result),
            () => Assert.Equal("Renamed Poule", _dbContext.Poules.Single(p => p.Id == new PouleId(_renamePouleId)).Name.Value)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public async Task SetTotalPlayersAsync_ReturnsNoContent_TotalPlayersUpdatedInDb()
    {
        // arrange
        var setTotalPlayersViewModel = new SetTotalPlayersPouleViewModel { TotalPlayers = 6 };

        // act
        var result = await CreateController().SetTotalPlayersAsync(_setTotalPlayersPouleId, setTotalPlayersViewModel);

        // assert
        Assert.Multiple(
            () => Assert.IsType<NoContentResult>(result),
            () => Assert.Equal(6, _dbContext.Poules.Single(p => p.Id == new PouleId(_setTotalPlayersPouleId)).TotalPlayers.Value)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public async Task DeleteAsync_ReturnsNoContent_PouleRemovedFromDb()
    {
        // act
        var result = await CreateController().DeleteAsync(_deletePouleId);

        // assert
        Assert.Multiple(
            () => Assert.IsType<NoContentResult>(result),
            () => Assert.Equal(_seedPouleCount - 1, _dbContext.Poules.Count())
        );
    }
}
