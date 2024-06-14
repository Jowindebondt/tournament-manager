using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class Poule10PlayerTemplateTestService
{
    private readonly Poule10PlayerTemplateService _service;

    public Poule10PlayerTemplateTestService()
    {
        _service = new Poule10PlayerTemplateService();
    }

    [Fact]
    [Trait(TraitCategories.TestLevel, TestLevels.UnitTest)]
    public void GetTemplate_Validate()
    {
        // arrange

        // act
        var template = _service.GetTemplate();

        // assert
        Assert.Multiple(
            () => Assert.NotNull(template),
            () => Assert.NotNull(template.Players),
            () => Assert.Equal(10, template.Players.Count),
            () => Assert.NotNull(template.Matches),
            () => Assert.Equal(45, template.Matches.Count)
        );
    }
}
