namespace EventBus.Events.Template;

public class GetTemplateEvent : IEvent
{
    public Guid Guid { get; }
    public Guid TemplateId { get; }

    public GetTemplateEvent(Guid guid, Guid templateId)
    {
        Guid = guid;
        TemplateId = templateId;
    }
}
