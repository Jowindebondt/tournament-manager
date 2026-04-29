using AutoMapper;
using Design.Application.Rounds.Commands;
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

public class CT_RoundController : IDisposable
{
    private readonly TestDesignDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    private readonly Guid _tournamentId;
    private readonly Guid _existingRoundId;
    private readonly Guid _renameRoundId;
    private readonly Guid _deleteRoundId;
    private readonly int _seedRoundCount;

    public CT_RoundController()
    {
        var options = new DbContextOptionsBuilder<TestDesignDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _dbContext = new TestDesignDbContext(options);

        _tournamentId = Guid.NewGuid();
        _existingRoundId = Guid.NewGuid();
        _renameRoundId = Guid.NewGuid();
        _deleteRoundId = Guid.NewGuid();
        _seedRoundCount = 3;

        var tournament = new Tournament(new TournamentId(_tournamentId), TournamentName.Create("Tournament A"), Sport.TableTennis);
        _dbContext.Tournaments.Add(tournament);

        _dbContext.Rounds.AddRange(
            new Round(new RoundId(_existingRoundId), RoundName.Create("Round A"), new TournamentId(_tournamentId)),
            new Round(new RoundId(_renameRoundId), RoundName.Create("Round B"), new TournamentId(_tournamentId)),
            new Round(new RoundId(_deleteRoundId), RoundName.Create("Round C"), new TournamentId(_tournamentId))
        );
        _dbContext.SaveChanges();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(typeof(global::Design.Api.MappingProfile), typeof(global::Design.Application.MappingProfile));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateRoundCommand).Assembly));
        services.AddSingleton<ITournamentRepository>(new TournamentRepository(_dbContext));
        services.AddSingleton<IRoundRepository>(new RoundRepository(_dbContext));
        services.AddSingleton<IPouleRepository>(new PouleRepository(_dbContext));
        var sp = services.BuildServiceProvider();
        _mapper = sp.GetRequiredService<IMapper>();
        _mediator = sp.GetRequiredService<IMediator>();
    }

    private global::Design.Api.Controllers.RoundController CreateController()
        => new global::Design.Api.Controllers.RoundController(_mapper, _mediator);

    public void Dispose() => _dbContext.Dispose();

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public async Task GetAllByTournamentAsync_ReturnsOk_WithFilledList()
    {
        // arrange
        OkObjectResult okResult = null!;
        IEnumerable<RoundViewModel> content = null!;

        // act
        var result = await CreateController().GetAllByTournamentAsync(_tournamentId);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => content = Assert.IsAssignableFrom<IEnumerable<RoundViewModel>>(okResult.Value),
            () => Assert.Equal(_seedRoundCount, content.Count())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public async Task GetByIdAsync_ExistingId_ReturnsOk()
    {
        // arrange
        OkObjectResult okResult = null!;
        RoundViewModel viewModel = null!;

        // act
        var result = await CreateController().GetByIdAsync(_existingRoundId);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => viewModel = Assert.IsType<RoundViewModel>(okResult.Value),
            () => Assert.Equal(_existingRoundId, viewModel.Id)
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
    public async Task CreateAsync_ValidInput_ReturnsCreatedAtAction_RoundAddedToDb()
    {
        // arrange
        var createViewModel = new CreateRoundViewModel { Name = "New Round", TournamentId = _tournamentId };
        CreatedAtActionResult createdResult = null!;
        RoundViewModel viewModel = null!;

        // act
        var result = await CreateController().CreateAsync(createViewModel);

        // assert
        Assert.Multiple(
            () => createdResult = Assert.IsType<CreatedAtActionResult>(result),
            () => Assert.NotNull(createdResult.Value),
            () => viewModel = Assert.IsType<RoundViewModel>(createdResult.Value),
            () => Assert.Equal("New Round", viewModel.Name),
            () => Assert.Equal(_seedRoundCount + 1, _dbContext.Rounds.Count())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public async Task RenameAsync_ReturnsNoContent_RoundRenamedInDb()
    {
        // arrange
        var renameViewModel = new RenameRoundViewModel { Name = "Renamed Round" };

        // act
        var result = await CreateController().RenameAsync(_renameRoundId, renameViewModel);

        // assert
        Assert.Multiple(
            () => Assert.IsType<NoContentResult>(result),
            () => Assert.Equal("Renamed Round", _dbContext.Rounds.Single(r => r.Id == new RoundId(_renameRoundId)).Name.Value)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public async Task DeleteAsync_ReturnsNoContent_RoundRemovedFromDb()
    {
        // act
        var result = await CreateController().DeleteAsync(_deleteRoundId);

        // assert
        Assert.Multiple(
            () => Assert.IsType<NoContentResult>(result),
            () => Assert.Equal(_seedRoundCount - 1, _dbContext.Rounds.Count())
        );
    }
}
