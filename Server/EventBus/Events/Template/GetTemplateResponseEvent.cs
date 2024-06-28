namespace EventBus.Events.Template;

public class GetTemplateResponseEvent : IEvent
{
    public Guid Guid { get; }
    public Guid TemplateId { get; }
    public TournamentTemplateModel Tournament { get; }

    public GetTemplateResponseEvent(Guid guid, Guid templateId, TournamentTemplateModel tournament)
    {
        Guid = guid;
        TemplateId = templateId;
        Tournament = tournament;
    }
}
