namespace EventBus.Events.Template;

public class TournamentTemplateModel
{
    public IEnumerable<RoundTemplateModel> Rounds { get; }

    public TournamentTemplateModel(IEnumerable<RoundTemplateModel> rounds)
    {
        Rounds = rounds;
    }
}
