using System.Reflection.Emit;
using Moq;
using TournamentManager.Domain;
using TournamentManager.Domain.Test;
using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class TableTennisPouleTestGenerator
{
    private readonly TableTennisPouleGenerator _generator;
    private readonly Mock<IPouleTemplateService> _mockTemplateService;
    private readonly Mock<PouleTemplateResolver> _mockTemplateResolver;
    private readonly Mock<IPouleService> _mockPouleService;

    public TableTennisPouleTestGenerator()
    {
        _mockTemplateResolver = new Mock<PouleTemplateResolver>();
        _mockTemplateResolver.Setup(resolve => resolve(It.IsAny<int>())).Returns(new Poule3PlayerTemplateService());
        _mockPouleService = new Mock<IPouleService>();
        _generator = new TableTennisPouleGenerator(_mockTemplateResolver.Object, _mockPouleService.Object);
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Generate_Handle_InsertCalledOnce()
    {
        // arrange
        var member1 = MemberBuilder.GetSingleMember();
        var member2 = MemberBuilder.GetSingleMember();
        var member3 = MemberBuilder.GetSingleMember();

        var tournament = TournamentBuilder.GetSingleTournament();
        tournament.Settings = new TableTennisSettings{
            TournamentType = TableTennisTournamentType.Single,
        };
        tournament.Members = [member1, member2, member3];
        tournament.Rounds = [RoundBuilder.GetSingleRound()];

        Poule newPoule = null!;
        _mockPouleService.Setup(service => service.Insert(It.IsAny<Poule>())).Callback<Poule>((poule) => { newPoule = poule; });

        // act
        _generator.Generate(tournament);
        
        // assert
        Assert.Multiple(
            () => _mockPouleService.Verify(service => service.Insert(It.IsAny<Poule>()), Times.Once),
            () => Assert.NotNull(newPoule)
        );
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void Generate_CantHandle_InsertCalledNever()
    {
        // arrange
        var tournament = TournamentBuilder.GetSingleTournament();

        // act
        _generator.Generate(tournament);
        
        // assert
        Assert.Multiple(
            () => _mockPouleService.Verify(service => service.Insert(It.IsAny<Poule>()), Times.Never)
        );
    }
}
