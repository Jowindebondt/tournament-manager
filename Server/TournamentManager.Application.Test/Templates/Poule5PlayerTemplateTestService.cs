using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class Poule5PlayerTemplateTestService
{
    private readonly Poule5PlayerTemplateService _service;

    public Poule5PlayerTemplateTestService()
    {
        _service = new Poule5PlayerTemplateService();
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
            () => Assert.Equal(5, template.Players.Count),
            () => Assert.NotNull(template.Matches),
            () => Assert.Equal(10, template.Matches.Count)
        );
    }
}
