namespace EventBus.Events.Template;

public class RoundTemplateModel
{
    public string Name { get; }
    public IEnumerable<PouleTemplateModel> Poules { get; }
    public RoundSettingsTemplateModel RoundSettings { get; }
    public RoundTemplateModel PreviousRound { get; set; }

    public RoundTemplateModel(string name, IEnumerable<PouleTemplateModel> poules, RoundSettingsTemplateModel roundSettings = null!, RoundTemplateModel previousRound = null!)
    {
        Name = name;
        Poules = poules;
        RoundSettings = roundSettings;
        PreviousRound = previousRound;
    }
}
