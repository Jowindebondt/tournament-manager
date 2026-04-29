using AutoMapper;
using Competition.Domain.Interfaces;
using Competition.Infrastructure.Repositories;
using Design.Application.Tournaments.Commands;
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

public class CT_TournamentController : IDisposable
{
    private readonly TestDesignDbContext _dbContext;
    private readonly TestCompetitionDbContext _competitionDbContext;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    private readonly Guid _existingId;
    private readonly Guid _renameId;
    private readonly Guid _deleteId;
    private readonly int _seedCount;

    public CT_TournamentController()
    {
        var designOptions = new DbContextOptionsBuilder<TestDesignDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _dbContext = new TestDesignDbContext(designOptions);

        var competitionOptions = new DbContextOptionsBuilder<TestCompetitionDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _competitionDbContext = new TestCompetitionDbContext(competitionOptions);

        _existingId = Guid.NewGuid();
        _renameId = Guid.NewGuid();
        _deleteId = Guid.NewGuid();
        _seedCount = 3;

        _dbContext.Tournaments.AddRange(
            new Tournament(new TournamentId(_existingId), TournamentName.Create("Tournament A"), Sport.TableTennis),
            new Tournament(new TournamentId(_renameId), TournamentName.Create("Tournament B"), Sport.TableTennis),
            new Tournament(new TournamentId(_deleteId), TournamentName.Create("Tournament C"), Sport.TableTennis)
        );
        _dbContext.SaveChanges();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(typeof(global::Design.Api.MappingProfile), typeof(global::Design.Application.MappingProfile));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateTournamentCommand).Assembly));
        services.AddSingleton<ITournamentRepository>(new TournamentRepository(_dbContext));
        services.AddSingleton<IRoundRepository>(new RoundRepository(_dbContext));
        services.AddSingleton<IPouleRepository>(new PouleRepository(_dbContext));
        services.AddSingleton<ICompetitionRepository>(new CompetitionRepository(_competitionDbContext));
        var sp = services.BuildServiceProvider();
        _mapper = sp.GetRequiredService<IMapper>();
        _mediator = sp.GetRequiredService<IMediator>();
    }

    private global::Design.Api.Controllers.TournamentController CreateController()
        => new global::Design.Api.Controllers.TournamentController(_mapper, _mediator);

    public void Dispose()
    {
        _dbContext.Dispose();
        _competitionDbContext.Dispose();
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public async Task GetAllAsync_ReturnsOk_WithFilledList()
    {
        // arrange
        OkObjectResult okResult = null!;
        IEnumerable<TournamentViewModel> content = null!;

        // act
        var result = await CreateController().GetAllAsync();

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => content = Assert.IsAssignableFrom<IEnumerable<TournamentViewModel>>(okResult.Value),
            () => Assert.Equal(_seedCount, content.Count())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public async Task GetByIdAsync_ExistingId_ReturnsOk()
    {
        // arrange
        OkObjectResult okResult = null!;
        TournamentViewModel viewModel = null!;

        // act
        var result = await CreateController().GetByIdAsync(_existingId);

        // assert
        Assert.Multiple(
            () => okResult = Assert.IsType<OkObjectResult>(result),
            () => Assert.NotNull(okResult.Value),
            () => viewModel = Assert.IsType<TournamentViewModel>(okResult.Value),
            () => Assert.Equal(_existingId, viewModel.Id)
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
    public async Task CreateAsync_ValidSport_ReturnsCreatedAtAction_TournamentAddedToDb()
    {
        // arrange
        var createViewModel = new CreateTournamentViewModel { Name = "New Tournament", Sport = "TableTennis" };
        CreatedAtActionResult createdResult = null!;
        TournamentViewModel viewModel = null!;

        // act
        var result = await CreateController().CreateAsync(createViewModel);

        // assert
        Assert.Multiple(
            () => createdResult = Assert.IsType<CreatedAtActionResult>(result),
            () => Assert.NotNull(createdResult.Value),
            () => viewModel = Assert.IsType<TournamentViewModel>(createdResult.Value),
            () => Assert.Equal("New Tournament", viewModel.Name),
            () => Assert.Equal(_seedCount + 1, _dbContext.Tournaments.Count())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public async Task CreateAsync_InvalidSport_ReturnsBadRequest_NothingAddedToDb()
    {
        // arrange
        var createViewModel = new CreateTournamentViewModel { Name = "New Tournament", Sport = "InvalidSport" };

        // act
        var result = await CreateController().CreateAsync(createViewModel);

        // assert
        Assert.Multiple(
            () => Assert.IsType<BadRequestObjectResult>(result),
            () => Assert.Equal(_seedCount, _dbContext.Tournaments.Count())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public async Task RenameAsync_ReturnsNoContent_TournamentRenamedInDb()
    {
        // arrange
        var renameViewModel = new RenameTournamentViewModel { Name = "Renamed Tournament" };

        // act
        var result = await CreateController().RenameAsync(_renameId, renameViewModel);

        // assert
        Assert.Multiple(
            () => Assert.IsType<NoContentResult>(result),
            () => Assert.Equal("Renamed Tournament", _dbContext.Tournaments.Find(new TournamentId(_renameId))!.Name.Value)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public async Task GenerateAsync_ReturnsNoContent_CompetitionAddedToDb()
    {
        // act
        var result = await CreateController().GenerateAsync(_existingId);

        // assert
        Assert.Multiple(
            () => Assert.IsType<NoContentResult>(result),
            () => Assert.Equal(1, _competitionDbContext.Competitions.Count())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public async Task GenerateAsync_CalledTwice_ReturnsNoContent_TwoCompetitionsAddedToDb()
    {
        // act
        var result1 = await CreateController().GenerateAsync(_existingId);
        var result2 = await CreateController().GenerateAsync(_existingId);

        // assert
        Assert.Multiple(
            () => Assert.IsType<NoContentResult>(result1),
            () => Assert.IsType<NoContentResult>(result2),
            () => Assert.Equal(2, _competitionDbContext.Competitions.Count())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.ComponentTest)]
    public async Task DeleteAsync_ReturnsNoContent_TournamentRemovedFromDb()
    {
        // act
        var result = await CreateController().DeleteAsync(_deleteId);

        // assert
        Assert.Multiple(
            () => Assert.IsType<NoContentResult>(result),
            () => Assert.Equal(_seedCount - 1, _dbContext.Tournaments.Count())
        );
    }
}
