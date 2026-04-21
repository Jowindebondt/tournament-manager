namespace Design.Api.ViewModels;

public class SetRoundPoulePositionViewModel
{
    public Guid PreviousPouleId { get; set; }
    public int PreviousPosition { get; set; }
    public Guid CurrentPouleId { get; set; }
    public int CurrentPosition { get; set; }
}
