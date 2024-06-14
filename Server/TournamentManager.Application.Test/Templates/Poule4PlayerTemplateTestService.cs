using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class Poule4PlayerTemplateTestService
{
    private readonly Poule4PlayerTemplateService _service;

    public Poule4PlayerTemplateTestService()
    {
        _service = new Poule4PlayerTemplateService();
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
            () => Assert.Equal(4, template.Players.Count),
            () => Assert.NotNull(template.Matches),
            () => Assert.Equal(6, template.Matches.Count)
        );
    }
}
