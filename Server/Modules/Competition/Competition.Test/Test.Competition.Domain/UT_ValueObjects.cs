using Competition.Domain.Enums;
using Competition.Domain.ValueObjects;
using Xunit;

namespace Test.Competition.Domain;

public class UT_ValueObjects
{
    // CompetitionName
    [Fact]
    public void CompetitionName_Create_ValidValue_ReturnsInstance()
    {
        var name = CompetitionName.Create("Competition A");
        Assert.Equal("Competition A", name.Value);
    }

    [Fact]
    public void CompetitionName_Create_EmptyValue_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => CompetitionName.Create(string.Empty));
    }

    [Fact]
    public void CompetitionName_Create_TooLongValue_ThrowsArgumentException()
    {
        var tooLong = new string('a', CompetitionName.MaxLength + 1);
        Assert.Throws<ArgumentException>(() => CompetitionName.Create(tooLong));
    }

    [Fact]
    public void CompetitionName_Create_MaxLengthValue_ReturnsInstance()
    {
        var maxLength = new string('a', CompetitionName.MaxLength);
        var name = CompetitionName.Create(maxLength);
        Assert.Equal(maxLength, name.Value);
    }

    // CompetitionRoundName
    [Fact]
    public void CompetitionRoundName_Create_ValidValue_ReturnsInstance()
    {
        var name = CompetitionRoundName.Create("Round A");
        Assert.Equal("Round A", name.Value);
    }

    [Fact]
    public void CompetitionRoundName_Create_EmptyValue_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => CompetitionRoundName.Create(string.Empty));
    }

    [Fact]
    public void CompetitionRoundName_Create_TooLongValue_ThrowsArgumentException()
    {
        var tooLong = new string('a', CompetitionRoundName.MaxLength + 1);
        Assert.Throws<ArgumentException>(() => CompetitionRoundName.Create(tooLong));
    }

    [Fact]
    public void CompetitionRoundName_Create_MaxLengthValue_ReturnsInstance()
    {
        var maxLength = new string('a', CompetitionRoundName.MaxLength);
        var name = CompetitionRoundName.Create(maxLength);
        Assert.Equal(maxLength, name.Value);
    }

    // CompetitionPouleName
    [Fact]
    public void CompetitionPouleName_Create_ValidValue_ReturnsInstance()
    {
        var name = CompetitionPouleName.Create("Poule A");
        Assert.Equal("Poule A", name.Value);
    }

    [Fact]
    public void CompetitionPouleName_Create_EmptyValue_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => CompetitionPouleName.Create(string.Empty));
    }

    [Fact]
    public void CompetitionPouleName_Create_TooLongValue_ThrowsArgumentException()
    {
        var tooLong = new string('a', CompetitionPouleName.MaxLength + 1);
        Assert.Throws<ArgumentException>(() => CompetitionPouleName.Create(tooLong));
    }

    [Fact]
    public void CompetitionPouleName_Create_MaxLengthValue_ReturnsInstance()
    {
        var maxLength = new string('a', CompetitionPouleName.MaxLength);
        var name = CompetitionPouleName.Create(maxLength);
        Assert.Equal(maxLength, name.Value);
    }

    // CompetitionId
    [Fact]
    public void CompetitionId_Constructor_StoresValue()
    {
        var guid = Guid.NewGuid();
        var id = new CompetitionId(guid);
        Assert.Equal(guid, id.Value);
    }

    // CompetitionRoundId
    [Fact]
    public void CompetitionRoundId_Constructor_StoresValue()
    {
        var guid = Guid.NewGuid();
        var id = new CompetitionRoundId(guid);
        Assert.Equal(guid, id.Value);
    }

    // CompetitionPouleId
    [Fact]
    public void CompetitionPouleId_Constructor_StoresValue()
    {
        var guid = Guid.NewGuid();
        var id = new CompetitionPouleId(guid);
        Assert.Equal(guid, id.Value);
    }

    // RoundRobinPlan
    [Fact]
    public void RoundRobinPlan_Constructor_CreatesInstance()
    {
        var plan = new RoundRobinPlan();
        Assert.NotNull(plan);
    }

    [Fact]
    public void RoundRobinPlan_TwoInstances_AreEqual()
    {
        var plan1 = new RoundRobinPlan();
        var plan2 = new RoundRobinPlan();
        Assert.Equal(plan1, plan2);
    }

    // KnockOutPlan
    [Fact]
    public void KnockOutPlan_Constructor_SemiFinal_StoresPhase()
    {
        var plan = new KnockOutPlan(KnockOutPhase.SemiFinal);
        Assert.Equal(KnockOutPhase.SemiFinal, plan.Phase);
    }

    [Fact]
    public void KnockOutPlan_Constructor_ThirdPlace_StoresPhase()
    {
        var plan = new KnockOutPlan(KnockOutPhase.ThirdPlace);
        Assert.Equal(KnockOutPhase.ThirdPlace, plan.Phase);
    }

    [Fact]
    public void KnockOutPlan_Constructor_Final_StoresPhase()
    {
        var plan = new KnockOutPlan(KnockOutPhase.Final);
        Assert.Equal(KnockOutPhase.Final, plan.Phase);
    }

    [Fact]
    public void KnockOutPlan_SamePhase_AreEqual()
    {
        var plan1 = new KnockOutPlan(KnockOutPhase.Final);
        var plan2 = new KnockOutPlan(KnockOutPhase.Final);
        Assert.Equal(plan1, plan2);
    }

    [Fact]
    public void KnockOutPlan_DifferentPhase_AreNotEqual()
    {
        var plan1 = new KnockOutPlan(KnockOutPhase.SemiFinal);
        var plan2 = new KnockOutPlan(KnockOutPhase.Final);
        Assert.NotEqual(plan1, plan2);
    }

    [Fact]
    public void KnockOutPlan_AndRoundRobinPlan_AreNotEqual()
    {
        CompetitionPlan plan1 = new KnockOutPlan(KnockOutPhase.Final);
        CompetitionPlan plan2 = new RoundRobinPlan();
        Assert.NotEqual(plan1, plan2);
    }
}
