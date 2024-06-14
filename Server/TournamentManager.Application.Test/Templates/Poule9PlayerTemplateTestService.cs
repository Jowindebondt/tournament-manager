using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class Poule9PlayerTemplateTestService
{
    private readonly Poule9PlayerTemplateService _service;

    public Poule9PlayerTemplateTestService()
    {
        _service = new Poule9PlayerTemplateService();
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
            () => Assert.Equal(9, template.Players.Count),
            () => Assert.NotNull(template.Matches),
            () => Assert.Equal(36, template.Matches.Count)
        );
    }
}
