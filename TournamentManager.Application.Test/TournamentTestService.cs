using Moq;
using TournamentManager.Application.Repositories;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using Xunit;

namespace TournamentManager.Application.Test;

public class TournamentTestService
{
    private readonly Mock<IRepository<Tournament>> _mockRepository;

    public TournamentTestService() 
    {
        _mockRepository = new Mock<IRepository<Tournament>>();
    }

    [Fact]
    public void Get_ReturnsInstance_RepoGetCalledOnce()
    {
        // arrange
        _mockRepository.Setup(repo => repo.Get(It.IsAny<int>())).Returns(TournamentBuilder.GetSingleTournament());
        var service = new TournamentService(_mockRepository.Object);

        // act
        var tournament = service.Get(-1);

        // assert
        _mockRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once());
    }

    [Fact]
    public void Get_ReturnsNull_RepoGetCalledOnce()
    {
        // arrange
        _mockRepository.Setup(repo => repo.Get(It.IsAny<int>())).Returns((Tournament)null);
        var service = new TournamentService(_mockRepository.Object);

        // act
        var tournament = service.Get(-1);

        // assert
        _mockRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once());
    }

    [Fact]
    public void GetAll_ReturnsFilledList_RepoGetAllCalledOnce()
    {
        // arrange
        _mockRepository.Setup(repo => repo.GetAll()).Returns(TournamentBuilder.GetListTournament(5));
        var service = new TournamentService(_mockRepository.Object);

        // act
        var tournaments = service.GetAll();

        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.GetAll(), Times.Once()),
            () => Assert.NotNull(tournaments),
            () => Assert.Equal(5, tournaments.Count())
        );
    }

    [Fact]
    public void GetAll_ReturnsNullOnEmptyList_RepoGetAllCalledOnce()
    {
        // arrange
        _mockRepository.Setup(repo => repo.GetAll()).Returns(TournamentBuilder.GetListTournament(0));
        var service = new TournamentService(_mockRepository.Object);

        // act
        var tournaments = service.GetAll();

        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.GetAll(), Times.Once()),
            () => Assert.Null(tournaments)
        );
    }

    [Fact]
    public void GetAll_ReturnsNull_RepoGetAllCalledOnce()
    {
        // arrange
        _mockRepository.Setup(repo => repo.GetAll()).Returns((IEnumerable<Tournament>)null);
        var service = new TournamentService(_mockRepository.Object);

        // act
        var tournaments = service.GetAll();

        // assert

        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.GetAll(), Times.Once()),
            () => Assert.Null(tournaments)
        );
    }

    [Fact]
    public void InsertValidInstance_ReturnsUpdatedInstance_RepoInsertCalledOnce()
    {
        // arrange
        var currentTimestamp = DateTime.UtcNow;
        var newInstance = new Tournament 
        {
            Name = "Test_Tournament_Insert"
        };
        _mockRepository.Setup(repo => repo.Insert(It.IsAny<Tournament>())).Callback(() => newInstance.Id = 1);
        var service = new TournamentService(_mockRepository.Object);

        // act
        service.Insert(newInstance);
        
        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.Insert(It.IsAny<Tournament>()), Times.Once),
            () => Assert.NotNull(newInstance),
            () => Assert.NotNull(newInstance.Id),
            () => Assert.NotNull(newInstance.CreatedDate),
            () => Assert.NotNull(newInstance.ModifiedDate),
            () => Assert.Equal(newInstance.CreatedDate, newInstance.ModifiedDate)
        );
    }

    [Fact]
    public void InsertValidInstanceWithId_ThrowsArgumentException_RepoInsertCalledNever()
    {
        // arrange
        var currentTimestamp = DateTime.UtcNow;
        var newInstance = new Tournament 
        {
            Id = 1,
            Name = "Test_Tournament_Insert"
        };
        var service = new TournamentService(_mockRepository.Object);
        
        Assert.Multiple(
            // act
            () => Assert.Throws<ArgumentException>(() => service.Insert(newInstance)),
    
            // assert
            () => _mockRepository.Verify(repo => repo.Insert(It.IsAny<Tournament>()), Times.Never)
        );
    }

    [Fact]
    public void InsertNoInstance_ThrowsArgumentNullException_RepoInsertCalledNever()
    {
        // arrange
        var service = new TournamentService(_mockRepository.Object);
        
        Assert.Multiple(
            // act
            () => Assert.Throws<ArgumentNullException>(() => service.Insert(null)),
    
            // assert
            () => _mockRepository.Verify(repo => repo.Insert(It.IsAny<Tournament>()), Times.Never)
        );
    }

    [Fact]
    public void UpdateValidInstance_ReturnsUpdatedInstance_RepoGetCalledOnce_RepoUpdateCalledOnce()
    {
        // arrange
        var existingInstance = TournamentBuilder.GetSingleTournament();
        var updatingInstance = TournamentBuilder.GetSingleTournament();
        updatingInstance.Name = "Test_Tournament_Update";
        var originalName = existingInstance.Name;
        var originalCreatedDate = existingInstance.CreatedDate;
        var originalModifiedDate = existingInstance.ModifiedDate;

        _mockRepository.Setup(repo => repo.Get(It.IsAny<int>())).Returns(existingInstance);
        _mockRepository.Setup(repo => repo.Update(It.IsAny<Tournament>())).Callback(() => {});
        var service = new TournamentService(_mockRepository.Object);

        // act
        var updatedInstance = service.Update(existingInstance.Id.Value, updatingInstance);
        
        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once),
            () => _mockRepository.Verify(repo => repo.Update(It.IsAny<Tournament>()), Times.Once),
            () => Assert.Equal(originalCreatedDate, existingInstance.CreatedDate),
            () => Assert.NotEqual(originalModifiedDate, existingInstance.ModifiedDate),
            () => Assert.Equal(updatingInstance.Name, updatedInstance.Name),
            () => Assert.NotEqual(originalName, updatedInstance.Name)
        );
    }

    [Fact]
    public void UpdateNoInstance_ThrowsArgumentNullException_RepoGetCalledNever_RepoUpdateCalledNever()
    {
        // arrange
        var currentTimestamp = DateTime.UtcNow;
        var existingInstance = TournamentBuilder.GetSingleTournament();

        var service = new TournamentService(_mockRepository.Object);

        Assert.Multiple(
            // act
            () => Assert.Throws<ArgumentNullException>(() => service.Update(existingInstance.Id.Value, null)),
        
            // assert
            () => _mockRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Never),
            () => _mockRepository.Verify(repo => repo.Update(It.IsAny<Tournament>()), Times.Never)
        );
    }

    [Fact]
    public void UpdateValidInstanceWithWrongId_ThrowsNullReferenceException_RepoGetCalledOnce_RepoUpdateCalledNever()
    {
        // arrange
        var existingInstance = TournamentBuilder.GetSingleTournament();
        var updatingInstance = TournamentBuilder.GetSingleTournament();
        updatingInstance.Name = "Test_Tournament_Update";
        var originalCreatedDate = existingInstance.CreatedDate;
        var originalModifiedDate = existingInstance.ModifiedDate;

        _mockRepository.Setup(repo => repo.Get(It.IsAny<int>())).Returns((Tournament)null);
        var service = new TournamentService(_mockRepository.Object);

        Assert.Multiple(
            // act
            () => Assert.Throws<NullReferenceException>(() => service.Update(existingInstance.Id.Value + 1, updatingInstance)),
        
            // assert
            () => _mockRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once),
            () => _mockRepository.Verify(repo => repo.Update(It.IsAny<Tournament>()), Times.Never),
            () => Assert.Equal(originalCreatedDate, existingInstance.CreatedDate),
            () => Assert.Equal(originalModifiedDate, existingInstance.ModifiedDate)
        );
    }

    [Fact]
    public void DeleteInstanceWithValidId_ThrowsNoException_RepoGetCalledOnce_RepoDeleteCalledOnce()
    {
        // arrange
        var existingInstance = TournamentBuilder.GetSingleTournament();

        _mockRepository.Setup(repo => repo.Get(It.IsAny<int>())).Returns(existingInstance);
        var service = new TournamentService(_mockRepository.Object);

        // act
        service.Delete(existingInstance.Id.Value);

        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once),
            () => _mockRepository.Verify(repo => repo.Delete(It.IsAny<Tournament>()), Times.Once)
        );
    }

    [Fact]
    public void DeleteInstanceWithInvalidId_ThrowsNoException_RepoGetCalledOnce_RepoDeleteCalledNever()
    {
        // arrange

        _mockRepository.Setup(repo => repo.Get(It.IsAny<int>())).Returns((Tournament)null);
        var service = new TournamentService(_mockRepository.Object);

        Assert.Multiple(
            // act
            () => Assert.Throws<NullReferenceException>(() => service.Delete(0)),

            // assert
            () => _mockRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once),
            () => _mockRepository.Verify(repo => repo.Delete(It.IsAny<Tournament>()), Times.Never)
        );
    }
}