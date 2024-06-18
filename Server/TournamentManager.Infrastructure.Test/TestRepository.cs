using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Moq;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Infrastructure.Test;

public class TestRepository
{
    private readonly Mock<DbSet<Tournament>> _dbSet;
    private readonly Mock<ApplicationDbContext> _dbContextMock;
    private readonly Repository<Tournament> _repo;

    public TestRepository() 
    {
        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().Options;

        _dbSet = new Mock<DbSet<Tournament>>();
        _dbContextMock = new Mock<ApplicationDbContext>(dbContextOptions);
        _dbContextMock.Setup(context => context.Set<Tournament>()).Returns(_dbSet.Object);
        _repo = new Repository<Tournament>(_dbContextMock.Object);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetValidInstance_DbSetFindCalledOnce_DbContextSaveChangesCalledNever()
    {
        // arrange
        _dbSet.Setup(set => set.Find(It.IsAny<int>())).Returns(TournamentBuilder.GetSingleTournament());

        // act
        var tournament = _repo.Get(-1);

        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.Find(It.IsAny<int>()), Times.Once()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Never())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetNoInstance_DbSetFindCalledOnce_DbContextSaveChangesCalledNever()
    {
        // arrange
        _dbSet.Setup(set => set.Find(It.IsAny<int>())).Returns((Tournament)null);

        // act
        var tournament = _repo.Get(-1);

        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.Find(It.IsAny<int>()), Times.Once()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Never())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetFilledList_DbSetAsQueryableCalledOnce_DbContextSaveChangesCalledNever()
    {
        // arrange
        _dbSet.Setup(set => set.AsQueryable()).Returns(TournamentBuilder.GetListTournamentAsQueryable(5));

        // act
        var tournaments = _repo.GetAll();

        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.AsQueryable(), Times.Once()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Never())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetEmptyList_DbSetAsQueryableCalledOnce_DbContextSaveChangesCalledNever()
    {
        // arrange
        _dbSet.Setup(set => set.AsQueryable()).Returns(TournamentBuilder.GetListTournamentAsQueryable(0));

        // act
        var tournaments = _repo.GetAll();

        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.AsQueryable(), Times.Once()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Never())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetNoList_DbSetAsQueryableCalledOnce_DbContextSaveChangesCalledNever()
    {
        // arrange
        _dbSet.Setup(set => set.AsQueryable()).Returns((IQueryable<Tournament>)null);

        // act
        var tournaments = _repo.GetAll();

        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.AsQueryable(), Times.Once()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Never())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void InsertWithValidInstance_DbSetAddCalledOnce_DbContextSaveChangesCalledOnce() 
    {
        // arrange
        
        // act
        _repo.Insert(TournamentBuilder.GetSingleTournament());

        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.Add(It.IsAny<Tournament>()), Times.Once()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Once())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void InsertWithNull_DbSetAddCalledNever_DbContextSaveChangesCalledNever_ThrowsArgumentNullException() 
    {
        // arrange
        
        Assert.Multiple(
            // act
            () => Assert.Throws<ArgumentNullException>(() => _repo.Insert(null)),

            // assert
            () => _dbSet.Verify(set => set.Add(It.IsAny<Tournament>()), Times.Never()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Never())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void UpdateWithValidInstance_DbSetAddCalledOnce_DbContextSaveChangesCalledOnce() 
    {
        // arrange
        
        // act
        _repo.Update(TournamentBuilder.GetSingleTournament());

        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.Update(It.IsAny<Tournament>()), Times.Once()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Once())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void UpdateWithNull_DbSetAddCalledNever_DbContextSaveChangesCalledNever_ThrowsArgumentNullException() 
    {
        // arrange
        
        Assert.Multiple(
            // act
            () => Assert.Throws<ArgumentNullException>(() => _repo.Update(null)),

            // assert
            () => _dbSet.Verify(set => set.Update(It.IsAny<Tournament>()), Times.Never()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Never())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void DeleteWithValidInstance_DbSetAddCalledOnce_DbContextSaveChangesCalledOnce() 
    {
        // arrange
        
        // act
        _repo.Delete(TournamentBuilder.GetSingleTournament());

        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.Remove(It.IsAny<Tournament>()), Times.Once()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Once())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void DeleteWithNull_DbSetAddCalledNever_DbContextSaveChangesCalledNever_ThrowsArgumentNullException() 
    {
        // arrange
        
        Assert.Multiple(
            // act
            () => Assert.Throws<ArgumentNullException>(() => _repo.Delete(null)),

            // assert
            () => _dbSet.Verify(set => set.Remove(It.IsAny<Tournament>()), Times.Never()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Never())
        );
    }
}
