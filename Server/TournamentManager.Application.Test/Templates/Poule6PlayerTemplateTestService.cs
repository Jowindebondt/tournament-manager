using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class Poule6PlayerTemplateTestService
{
    private readonly Poule6PlayerTemplateService _service;

    public Poule6PlayerTemplateTestService()
    {
        _service = new Poule6PlayerTemplateService();
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
            () => Assert.Equal(6, template.Players.Count),
            () => Assert.NotNull(template.Matches),
            () => Assert.Equal(15, template.Matches.Count)
        );
    }
}
