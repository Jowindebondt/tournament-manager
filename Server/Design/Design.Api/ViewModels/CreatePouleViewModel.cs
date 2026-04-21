namespace Design.Api.ViewModels;

public class CreatePouleViewModel
{
    public string Name { get; set; }
    public int TotalPlayers { get; set; }
    public Guid RoundId { get; set; }
}
