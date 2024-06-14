using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class Poule11PlayerTemplateTestService
{
    private readonly Poule11PlayerTemplateService _service;

    public Poule11PlayerTemplateTestService()
    {
        _service = new Poule11PlayerTemplateService();
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
            () => Assert.Equal(11, template.Players.Count),
            () => Assert.NotNull(template.Matches),
            () => Assert.Equal(55, template.Matches.Count)
        );
    }
}
