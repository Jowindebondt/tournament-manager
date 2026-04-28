namespace Design.Application.DTOs;

public class SetRoundPoulePositionDto
{
    public Guid Id { get; set; }
    public Guid PreviousPouleId { get; set; }
    public int PreviousPosition { get; set; }
    public Guid CurrentPouleId { get; set; }
    public int CurrentPosition { get; set; }
}
