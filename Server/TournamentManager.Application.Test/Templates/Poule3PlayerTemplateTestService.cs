using TournamentManager.TestHelper;
using Xunit;

namespace TournamentManager.Application.Test;

public class Poule3PlayerTemplateTestService
{
    private readonly Poule3PlayerTemplateService _service;

    public Poule3PlayerTemplateTestService()
    {
        _service = new Poule3PlayerTemplateService();
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
            () => Assert.Equal(3, template.Players.Count),
            () => Assert.NotNull(template.Matches),
            () => Assert.Equal(3, template.Matches.Count)
        );
    }
}
