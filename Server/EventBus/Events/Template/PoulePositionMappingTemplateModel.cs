namespace EventBus;

public class PoulePositionMappingTemplateModel
{
    public PoulePositionTemplateModel Previous { get; }
    public PoulePositionTemplateModel Current { get; }

    public PoulePositionMappingTemplateModel(PoulePositionTemplateModel previous, PoulePositionTemplateModel current)
    {
        Previous = previous;
        Current = current;
    }
}
