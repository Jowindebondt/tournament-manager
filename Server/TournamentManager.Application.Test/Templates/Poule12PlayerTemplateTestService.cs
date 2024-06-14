using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class Poule12PlayerTemplateTestService
{
    private readonly Poule12PlayerTemplateService _service;

    public Poule12PlayerTemplateTestService()
    {
        _service = new Poule12PlayerTemplateService();
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
            () => Assert.Equal(12, template.Players.Count),
            () => Assert.NotNull(template.Matches),
            () => Assert.Equal(66, template.Matches.Count)
        );
    }
}
