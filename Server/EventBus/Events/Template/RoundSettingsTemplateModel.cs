namespace EventBus;

public class RoundSettingsTemplateModel
{
    public IEnumerable<PoulePositionMappingTemplateModel> PoulePositionMapping { get; }

    public RoundSettingsTemplateModel(IEnumerable<PoulePositionMappingTemplateModel> poulePositionMapping)
    {
        PoulePositionMapping = poulePositionMapping;
    }
}
