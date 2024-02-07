using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Moq;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using Xunit;

namespace TournamentManager.Infrastructure.Test;

public class TestRepository
{
    private readonly Mock<DbSet<Tournament>> _dbSet;
    private readonly Mock<ApplicationDbContext> _dbContextMock;

    public TestRepository() 
    {
        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().Options;

        _dbSet = new Mock<DbSet<Tournament>>();
        _dbContextMock = new Mock<ApplicationDbContext>(dbContextOptions);
        _dbContextMock.Setup(context => context.Set<Tournament>()).Returns(_dbSet.Object);
    }

    [Fact]
    public void GetValidInstance_DbSetFindCalledOnce_DbContextSaveChangesCalledNever()
    {
        // arrange
        _dbSet.Setup(set => set.Find(It.IsAny<int>())).Returns(TournamentBuilder.GetSingleTournament());
        var repo = new Repository<Tournament>(_dbContextMock.Object);

        // act
        var tournament = repo.Get(-1);

        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.Find(It.IsAny<int>()), Times.Once()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Never())
        );
    }

    [Fact]
    public void GetNoInstance_DbSetFindCalledOnce_DbContextSaveChangesCalledNever()
    {
        // arrange
        _dbSet.Setup(set => set.Find(It.IsAny<int>())).Returns((Tournament)null);
        var repo = new Repository<Tournament>(_dbContextMock.Object);

        // act
        var tournament = repo.Get(-1);

        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.Find(It.IsAny<int>()), Times.Once()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Never())
        );
    }

    [Fact]
    public void GetFilledList_DbSetAsQueryableCalledOnce_DbContextSaveChangesCalledNever()
    {
        // arrange
        _dbSet.Setup(set => set.AsQueryable()).Returns(TournamentBuilder.GetListTournamentAsQueryable(5));
        var repo = new Repository<Tournament>(_dbContextMock.Object);

        // act
        var tournaments = repo.GetAll();

        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.AsQueryable(), Times.Once()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Never())
        );
    }

    [Fact]
    public void GetEmptyList_DbSetAsQueryableCalledOnce_DbContextSaveChangesCalledNever()
    {
        // arrange
        _dbSet.Setup(set => set.AsQueryable()).Returns(TournamentBuilder.GetListTournamentAsQueryable(0));
        var repo = new Repository<Tournament>(_dbContextMock.Object);

        // act
        var tournaments = repo.GetAll();

        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.AsQueryable(), Times.Once()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Never())
        );
    }

    [Fact]
    public void GetNoList_DbSetAsQueryableCalledOnce_DbContextSaveChangesCalledNever()
    {
        // arrange
        _dbSet.Setup(set => set.AsQueryable()).Returns((IQueryable<Tournament>)null);
        var repo = new Repository<Tournament>(_dbContextMock.Object);

        // act
        var tournaments = repo.GetAll();

        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.AsQueryable(), Times.Once()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Never())
        );
    }

    [Fact]
    public void InsertWithValidInstance_DbSetAddCalledOnce_DbContextSaveChangesCalledOnce() 
    {
        // arrange
        var repo = new Repository<Tournament>(_dbContextMock.Object);
        
        // act
        repo.Insert(TournamentBuilder.GetSingleTournament());

        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.Add(It.IsAny<Tournament>()), Times.Once()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Once())
        );
    }

    [Fact]
    public void InsertWithNull_DbSetAddCalledNever_DbContextSaveChangesCalledNever_ThrowsArgumentNullException() 
    {
        // arrange
        var repo = new Repository<Tournament>(_dbContextMock.Object);
        
        Assert.Multiple(
            // act
            () => Assert.Throws<ArgumentNullException>(() => repo.Insert(null)),

            // assert
            () => _dbSet.Verify(set => set.Add(It.IsAny<Tournament>()), Times.Never()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Never())
        );
    }

    [Fact]
    public void UpdateWithValidInstance_DbSetAddCalledOnce_DbContextSaveChangesCalledOnce() 
    {
        // arrange
        var repo = new Repository<Tournament>(_dbContextMock.Object);
        
        // act
        repo.Update(TournamentBuilder.GetSingleTournament());

        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.Update(It.IsAny<Tournament>()), Times.Once()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Once())
        );
    }

    [Fact]
    public void UpdateWithNull_DbSetAddCalledNever_DbContextSaveChangesCalledNever_ThrowsArgumentNullException() 
    {
        // arrange
        var repo = new Repository<Tournament>(_dbContextMock.Object);
        
        Assert.Multiple(
            // act
            () => Assert.Throws<ArgumentNullException>(() => repo.Update(null)),

            // assert
            () => _dbSet.Verify(set => set.Update(It.IsAny<Tournament>()), Times.Never()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Never())
        );
    }

    [Fact]
    public void DeleteWithValidInstance_DbSetAddCalledOnce_DbContextSaveChangesCalledOnce() 
    {
        // arrange
        var repo = new Repository<Tournament>(_dbContextMock.Object);
        
        // act
        repo.Delete(TournamentBuilder.GetSingleTournament());

        // assert
        Assert.Multiple(
            () => _dbSet.Verify(set => set.Remove(It.IsAny<Tournament>()), Times.Once()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Once())
        );
    }

    [Fact]
    public void DeleteWithNull_DbSetAddCalledNever_DbContextSaveChangesCalledNever_ThrowsArgumentNullException() 
    {
        // arrange
        var repo = new Repository<Tournament>(_dbContextMock.Object);
        
        Assert.Multiple(
            // act
            () => Assert.Throws<ArgumentNullException>(() => repo.Delete(null)),

            // assert
            () => _dbSet.Verify(set => set.Remove(It.IsAny<Tournament>()), Times.Never()),
            () => _dbContextMock.Verify(context => context.SaveChanges(), Times.Never())
        );
    }
}
