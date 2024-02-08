using Moq;
using TournamentManager.Application.Repositories;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class RoundTestService
{
    private readonly Mock<IRepository<Round>> _mockRepository;
    private readonly Mock<ITournamentService> _mockTournamentService;
    private readonly RoundService _roundService;

    public RoundTestService()
    {
        _mockRepository = new Mock<IRepository<Round>>();
        _mockTournamentService = new Mock<ITournamentService>();
        _roundService = new RoundService(_mockTournamentService.Object, _mockRepository.Object);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Get_ReturnsInstance_RepoGetCalledOnce()
    {
        // arrange
        _mockRepository.Setup(repo => repo.Get(It.IsAny<int>())).Returns(RoundBuilder.GetSingleRound());

        // act
        var tournament = _roundService.Get(-1);

        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once()),
            () => Assert.NotNull(tournament)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Get_ReturnsNull_RepoGetCalledOnce()
    {
        // arrange
        _mockRepository.Setup(repo => repo.Get(It.IsAny<int>())).Returns((Round)null);

        // act
        var tournament = _roundService.Get(-1);

        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once()),
            () => Assert.Null(tournament)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetAll_ReturnsFilledList_RepoGetAllCalledOnce()
    {
        // arrange
        _mockRepository.Setup(repo => repo.GetAll()).Returns(RoundBuilder.GetListRound(5, 1));

        // act
        var tournaments = _roundService.GetAll(-1);

        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.GetAll(), Times.Once()),
            () => Assert.NotNull(tournaments),
            () => Assert.Equal(5, tournaments.Count())
        );
    }

    [Theory]
    [InlineData(0,2)]
    [InlineData(5,2)]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetAll_ReturnsNullOnEmptyList_RepoGetAllCalledOnce(int count, int tournamentId)
    {
        // arrange
        _mockRepository.Setup(repo => repo.GetAll()).Returns(RoundBuilder.GetListRound(count, tournamentId));

        // act
        var tournaments = _roundService.GetAll(-1);

        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.GetAll(), Times.Once()),
            () => Assert.Null(tournaments)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetAll_ReturnsNull_RepoGetAllCalledOnce()
    {
        // arrange
        _mockRepository.Setup(repo => repo.GetAll()).Returns((IEnumerable<Round>)null);

        // act
        var tournaments = _roundService.GetAll(-1);

        // assert

        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.GetAll(), Times.Once()),
            () => Assert.Null(tournaments)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void InsertValidInstance_ReturnsUpdatedInstance_RepoInsertCalledOnce_ServiceGetCalledOnce()
    {
        // arrange
        var newInstance = new Round 
        {
            Name = "Test_Round_Insert"
        };
        _mockRepository.Setup(repo => repo.Insert(It.IsAny<Round>())).Callback(() => newInstance.Id = 1);
        _mockTournamentService.Setup(service => service.Get(It.IsAny<int>())).Returns(TournamentBuilder.GetSingleTournament());

        // act
        _roundService.Insert(-1, newInstance);
        
        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.Insert(It.IsAny<Round>()), Times.Once),
            () => _mockTournamentService.Verify(service => service.Get(It.IsAny<int>()), Times.Once),
            () => Assert.NotNull(newInstance),
            () => Assert.NotNull(newInstance.Id),
            () => Assert.NotNull(newInstance.CreatedDate),
            () => Assert.NotNull(newInstance.ModifiedDate),
            () => Assert.Equal(newInstance.CreatedDate, newInstance.ModifiedDate)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void InsertValidInstanceWithId_ThrowsArgumentException_RepoInsertCalledNever_ServiceGetCalledNever()
    {
        // arrange
        var newInstance = new Round 
        {
            Id = 1,
            Name = "Test_Round_Insert"
        };
        
        Assert.Multiple(
            // act
            () => Assert.Throws<ArgumentException>(() => _roundService.Insert(-1, newInstance)),
    
            // assert
            () => _mockRepository.Verify(repo => repo.Insert(It.IsAny<Round>()), Times.Never),
            () => _mockTournamentService.Verify(service => service.Get(It.IsAny<int>()), Times.Never)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void InsertValidInstanceWithNonExistingTournament_ThrowsNullReferenceException_RepoInsertCalledNever_ServiceGetCalledOnce()
    {
        // arrange
        var newInstance = new Round 
        {
            Name = "Test_Round_Insert"
        };
        _mockTournamentService.Setup(service => service.Get(It.IsAny<int>())).Returns((Tournament)null);

        Assert.Multiple(
            // act
            () => Assert.Throws<NullReferenceException>(() => _roundService.Insert(-1, newInstance)),
    
            // assert
            () => _mockRepository.Verify(repo => repo.Insert(It.IsAny<Round>()), Times.Never),
            () => _mockTournamentService.Verify(service => service.Get(It.IsAny<int>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void InsertNoInstance_ThrowsArgumentNullException_RepoInsertCalledNever_ServiceGetCalledNever()
    {
        // arrange
        
        Assert.Multiple(
            // act
            () => Assert.Throws<ArgumentNullException>(() => _roundService.Insert(-1, null)),
    
            // assert
            () => _mockRepository.Verify(repo => repo.Insert(It.IsAny<Round>()), Times.Never),
            () => _mockTournamentService.Verify(service => service.Get(It.IsAny<int>()), Times.Never)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void UpdateValidInstance_ReturnsUpdatedInstance_RepoGetCalledOnce_RepoUpdateCalledOnce()
    {
        // arrange
        var existingInstance = RoundBuilder.GetSingleRound();
        var updatingInstance = RoundBuilder.GetSingleRound();
        updatingInstance.Name = "Test_Round_Update";
        var originalName = existingInstance.Name;
        var originalCreatedDate = existingInstance.CreatedDate;
        var originalModifiedDate = existingInstance.ModifiedDate;

        _mockRepository.Setup(repo => repo.Get(It.IsAny<int>())).Returns(existingInstance);
        _mockRepository.Setup(repo => repo.Update(It.IsAny<Round>())).Callback(() => {});

        // act
        var updatedInstance = _roundService.Update(existingInstance.Id.Value, updatingInstance);
        
        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once),
            () => _mockRepository.Verify(repo => repo.Update(It.IsAny<Round>()), Times.Once),
            () => Assert.Equal(originalCreatedDate, existingInstance.CreatedDate),
            () => Assert.NotEqual(originalModifiedDate, existingInstance.ModifiedDate),
            () => Assert.Equal(updatingInstance.Name, updatedInstance.Name),
            () => Assert.NotEqual(originalName, updatedInstance.Name)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void UpdateNoInstance_ThrowsArgumentNullException_RepoGetCalledNever_RepoUpdateCalledNever()
    {
        // arrange
        var existingInstance = RoundBuilder.GetSingleRound();

        Assert.Multiple(
            // act
            () => Assert.Throws<ArgumentNullException>(() => _roundService.Update(existingInstance.Id.Value, null)),
        
            // assert
            () => _mockRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Never),
            () => _mockRepository.Verify(repo => repo.Update(It.IsAny<Round>()), Times.Never)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void UpdateValidInstanceWithWrongId_ThrowsNullReferenceException_RepoGetCalledOnce_RepoUpdateCalledNever()
    {
        // arrange
        var existingInstance = RoundBuilder.GetSingleRound();
        var updatingInstance = RoundBuilder.GetSingleRound();
        updatingInstance.Name = "Test_Round_Update";
        var originalCreatedDate = existingInstance.CreatedDate;
        var originalModifiedDate = existingInstance.ModifiedDate;

        _mockRepository.Setup(repo => repo.Get(It.IsAny<int>())).Returns((Round)null);

        Assert.Multiple(
            // act
            () => Assert.Throws<NullReferenceException>(() => _roundService.Update(existingInstance.Id.Value + 1, updatingInstance)),
        
            // assert
            () => _mockRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once),
            () => _mockRepository.Verify(repo => repo.Update(It.IsAny<Round>()), Times.Never),
            () => Assert.Equal(originalCreatedDate, existingInstance.CreatedDate),
            () => Assert.Equal(originalModifiedDate, existingInstance.ModifiedDate)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void DeleteInstanceWithValidId_ThrowsNoException_RepoGetCalledOnce_RepoDeleteCalledOnce()
    {
        // arrange
        var existingInstance = RoundBuilder.GetSingleRound();

        _mockRepository.Setup(repo => repo.Get(It.IsAny<int>())).Returns(existingInstance);

        // act
        _roundService.Delete(existingInstance.Id.Value);

        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once),
            () => _mockRepository.Verify(repo => repo.Delete(It.IsAny<Round>()), Times.Once)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void DeleteInstanceWithInvalidId_ThrowsNoException_RepoGetCalledOnce_RepoDeleteCalledNever()
    {
        // arrange
        _mockRepository.Setup(repo => repo.Get(It.IsAny<int>())).Returns((Round)null);

        Assert.Multiple(
            // act
            () => Assert.Throws<NullReferenceException>(() => _roundService.Delete(0)),

            // assert
            () => _mockRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once),
            () => _mockRepository.Verify(repo => repo.Delete(It.IsAny<Round>()), Times.Never)
        );
    }
}
