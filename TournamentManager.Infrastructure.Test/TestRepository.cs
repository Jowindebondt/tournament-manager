using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Moq;
using TournamentManager.Domain;
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
        _dbSet.Setup(set => set.Find(It.IsAny<int>())).Returns(GetSingleTournament(1));
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
    public void GetFilledList_DbSetAsEnumerableCalledOnce_DbContextSaveChangesCalledNever()
    {
        // arrange
        _dbSet.Setup(set => set.AsQueryable()).Returns(GetListTournament(5));
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
    public void GetEmptyList_DbSetAsEnumerableCalledOnce_DbContextSaveChangesCalledNever()
    {
        // arrange
        _dbSet.Setup(set => set.AsQueryable()).Returns(GetListTournament(0));
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
    public void GetNoList_DbSetAsEnumerableCalledOnce_DbContextSaveChangesCalledNever()
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
        repo.Insert(GetSingleTournament(1));

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
        repo.Update(GetSingleTournament(1));

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
        repo.Delete(GetSingleTournament(1));

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

    private static Tournament GetSingleTournament(int id)
    {
        return new Tournament
        {
            Id = id * -1,
            Name = $"Test_Tournament_{id}",
            CreatedDate = new DateTime(2024, 1, 1),
            ModifiedDate = new DateTime(2024, 1, 1),
        };
    }

    private static IQueryable<Tournament> GetListTournament(int count)
    {
        var arr = new Tournament[count];
        for (var i = 0; i < count; i++)
        {
            arr[i] = GetSingleTournament(i + 1);
        }

        return arr.AsQueryable();
    }
}