using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class Poule8PlayerTemplateTestService
{
    private readonly Poule8PlayerTemplateService _service;

    public Poule8PlayerTemplateTestService()
    {
        _service = new Poule8PlayerTemplateService();
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
            () => Assert.Equal(8, template.Players.Count),
            () => Assert.NotNull(template.Matches),
            () => Assert.Equal(28, template.Matches.Count)
        );
    }
}
