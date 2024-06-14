using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class Poule7PlayerTemplateTestService
{
    private readonly Poule7PlayerTemplateService _service;

    public Poule7PlayerTemplateTestService()
    {
        _service = new Poule7PlayerTemplateService();
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
            () => Assert.Equal(7, template.Players.Count),
            () => Assert.NotNull(template.Matches),
            () => Assert.Equal(21, template.Matches.Count)
        );
    }
}
