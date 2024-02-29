using Moq;
using TournamentManager.Application.Repositories;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class CrudTestService
{
    private readonly Mock<IRepository<Round>> _mockRepository;
    private readonly CrudService<Round> _service;

    public CrudTestService() 
    {
        _mockRepository = new Mock<IRepository<Round>>();
        _service = new CrudService<Round>(_mockRepository.Object);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetAll_ReturnsFilledList_RepoGetAllCalledOnce()
    {
        // arrange
        _mockRepository.Setup(repo => repo.GetAll()).Returns(RoundBuilder.GetListRound(5));

        // act
        var tournaments = _service.GetAll();

        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.GetAll(), Times.Once()),
            () => Assert.NotNull(tournaments),
            () => Assert.Equal(5, tournaments.Count())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetAllWithFilter_ReturnsFilledList_RepoGetAllCalledOnce()
    {
        // arrange
        _mockRepository.Setup(repo => repo.GetAll()).Returns(RoundBuilder.GetListRound(5, 1).Concat(RoundBuilder.GetListRound(5, 2)));

        // act
        var tournaments = _service.GetAll(u => u.Tournament.Id == -1);

        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.GetAll(), Times.Once()),
            () => Assert.NotNull(tournaments),
            () => Assert.Equal(5, tournaments.Count())
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetAll_ReturnsNullOnEmptyList_RepoGetAllCalledOnce()
    {
        // arrange
        _mockRepository.Setup(repo => repo.GetAll()).Returns(RoundBuilder.GetListRound(0));

        // act
        var tournaments = _service.GetAll();

        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.GetAll(), Times.Once()),
            () => Assert.Null(tournaments)
        );
    }

    [Theory]
    [InlineData(0,2)]
    [InlineData(5,2)]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetAllWithFilter_ReturnsNullOnEmptyList_RepoGetAllCalledOnce(int count, int tournamentId)
    {
        // arrange
        _mockRepository.Setup(repo => repo.GetAll()).Returns(RoundBuilder.GetListRound(count, tournamentId).Concat(RoundBuilder.GetListRound(count, tournamentId + 1)));

        // act
        var tournaments = _service.GetAll(u => u.Tournament.Id == tournamentId);

        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.GetAll(), Times.Once()),
            () => Assert.Null(tournaments)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetAll_ReturnsEmptyList_RepoGetAllCalledOnce()
    {
        // arrange
        _mockRepository.Setup(repo => repo.GetAll()).Returns([]);

        // act
        var tournaments = _service.GetAll();

        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.GetAll(), Times.Once()),
            () => Assert.Null(tournaments)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetAllWithFilter_ReturnsEmptyList_RepoGetAllCalledOnce()
    {
        // arrange
        _mockRepository.Setup(repo => repo.GetAll()).Returns([]);

        // act
        var tournaments = _service.GetAll(u => u.Tournament.Id == -1);

        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.GetAll(), Times.Once()),
            () => Assert.Null(tournaments)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Get_ReturnsInstance_RepoGetCalledOnce()
    {
        // arrange
        _mockRepository.Setup(repo => repo.Get(It.IsAny<int>())).Returns(RoundBuilder.GetSingleRound());

        // act
        var tournament = _service.Get(-1);

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
        var tournament = _service.Get(-1);

        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once()),
            () => Assert.Null(tournament)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void InsertValidInstance_ReturnsUpdatedInstance_RepoInsertCalledOnce()
    {
        // arrange
        var currentTimestamp = DateTime.UtcNow;
        var newInstance = new Round 
        {
            Name = "Test_Round_Insert"
        };
        _mockRepository.Setup(repo => repo.Insert(It.IsAny<Round>())).Callback(() => newInstance.Id = 1);

        // act
        _service.Insert(newInstance);
        
        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.Insert(It.IsAny<Round>()), Times.Once),
            () => Assert.NotNull(newInstance),
            () => Assert.NotNull(newInstance.Id),
            () => Assert.NotNull(newInstance.CreatedDate),
            () => Assert.NotNull(newInstance.ModifiedDate),
            () => Assert.Equal(newInstance.CreatedDate, newInstance.ModifiedDate)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void InsertValidInstanceWithTypeSpecifics_ReturnsUpdatedInstance_RepoInsertCalledOnce_TypeSpecificActionCalledOnce()
    {
        // arrange
        var newInstance = new Round 
        {
            Name = "Test_Round_Insert"
        };
        _mockRepository.Setup(repo => repo.Insert(It.IsAny<Round>())).Callback(() => newInstance.Id = 1);

        var typeSpecificMock = new Mock<Action>();
        typeSpecificMock.Setup(action => action.Invoke()).Callback(() => {});

        // act
        _service.Insert(newInstance, typeSpecificMock.Object);
        
        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.Insert(It.IsAny<Round>()), Times.Once),
            () => typeSpecificMock.Verify(action => action.Invoke(), Times.Once),
            () => Assert.NotNull(newInstance),
            () => Assert.NotNull(newInstance.Id),
            () => Assert.NotNull(newInstance.CreatedDate),
            () => Assert.NotNull(newInstance.ModifiedDate),
            () => Assert.Equal(newInstance.CreatedDate, newInstance.ModifiedDate)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void InsertValidInstanceWithId_ThrowsArgumentException_RepoInsertCalledNever()
    {
        // arrange
        var currentTimestamp = DateTime.UtcNow;
        var newInstance = new Round 
        {
            Id = 1,
            Name = "Test_Round_Insert"
        };
        
        Assert.Multiple(
            // act
            () => Assert.Throws<ArgumentException>(() => _service.Insert(newInstance)),
    
            // assert
            () => _mockRepository.Verify(repo => repo.Insert(It.IsAny<Round>()), Times.Never)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void InsertNoInstance_ThrowsArgumentNullException_RepoInsertCalledNever()
    {
        // arrange
        
        Assert.Multiple(
            // act
            () => Assert.Throws<ArgumentNullException>(() => _service.Insert(null)),
    
            // assert
            () => _mockRepository.Verify(repo => repo.Insert(It.IsAny<Round>()), Times.Never)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void UpdateValidInstance_ReturnsUpdatedInstance_RepoGetCalledOnce_RepoUpdateCalledOnce()
    {
        // arrange
        var existingInstance = RoundBuilder.GetSingleRound();
        var updatingInstance = RoundBuilder.GetSingleRound();
        var originalCreatedDate = existingInstance.CreatedDate;
        var originalModifiedDate = existingInstance.ModifiedDate;

        _mockRepository.Setup(repo => repo.Get(It.IsAny<int>())).Returns(existingInstance);
        _mockRepository.Setup(repo => repo.Update(It.IsAny<Round>())).Callback(() => {});

        var updateTypeSpecificsMock = new Mock<Action<Round>>();
        updateTypeSpecificsMock.Setup(action => action.Invoke(It.IsAny<Round>())).Callback(() => {});

        // act
        var updatedInstance = _service.Update(existingInstance.Id.Value, updatingInstance, updateTypeSpecificsMock.Object);
        
        // assert
        Assert.Multiple(
            () => _mockRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once),
            () => _mockRepository.Verify(repo => repo.Update(It.IsAny<Round>()), Times.Once),
            () => updateTypeSpecificsMock.Verify(action => action.Invoke(It.IsAny<Round>()), Times.Once),
            () => Assert.Equal(originalCreatedDate, existingInstance.CreatedDate),
            () => Assert.NotEqual(originalModifiedDate, existingInstance.ModifiedDate)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void UpdateNoInstance_ThrowsArgumentNullException_RepoGetCalledNever_RepoUpdateCalledNever()
    {
        // arrange
        var existingInstance = RoundBuilder.GetSingleRound();

        var updateTypeSpecificsMock = new Mock<Action<Round>>();
        updateTypeSpecificsMock.Setup(action => action.Invoke(It.IsAny<Round>())).Callback(() => {});

        Assert.Multiple(
            // act
            () => Assert.Throws<ArgumentNullException>(() => _service.Update(existingInstance.Id.Value, null, updateTypeSpecificsMock.Object)),
        
            // assert
            () => _mockRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Never),
            () => _mockRepository.Verify(repo => repo.Update(It.IsAny<Round>()), Times.Never),
            () => updateTypeSpecificsMock.Verify(action => action.Invoke(It.IsAny<Round>()), Times.Never)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void UpdateValidInstanceWithWrongId_ThrowsNullReferenceException_RepoGetCalledOnce_RepoUpdateCalledNever()
    {
        // arrange
        var existingInstance = RoundBuilder.GetSingleRound();
        var updatingInstance = RoundBuilder.GetSingleRound();
        var originalCreatedDate = existingInstance.CreatedDate;
        var originalModifiedDate = existingInstance.ModifiedDate;

        _mockRepository.Setup(repo => repo.Get(It.IsAny<int>())).Returns((Round)null);
        var updateTypeSpecificsMock = new Mock<Action<Round>>();
        updateTypeSpecificsMock.Setup(action => action.Invoke(It.IsAny<Round>())).Callback(() => {});

        Assert.Multiple(
            // act
            () => Assert.Throws<NullReferenceException>(() => _service.Update(existingInstance.Id.Value + 1, updatingInstance, updateTypeSpecificsMock.Object)),
        
            // assert
            () => _mockRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once),
            () => _mockRepository.Verify(repo => repo.Update(It.IsAny<Round>()), Times.Never),
            () => updateTypeSpecificsMock.Verify(action => action.Invoke(It.IsAny<Round>()), Times.Never),
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
        _service.Delete(existingInstance.Id.Value);

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
            () => Assert.Throws<NullReferenceException>(() => _service.Delete(0)),

            // assert
            () => _mockRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once),
            () => _mockRepository.Verify(repo => repo.Delete(It.IsAny<Round>()), Times.Never)
        );
    }
}
