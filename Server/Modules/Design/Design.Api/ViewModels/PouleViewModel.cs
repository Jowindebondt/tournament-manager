namespace Design.Api.ViewModels;

public class PouleViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TotalPlayers { get; set; }
    public RoundViewModel Round { get; set; } = null!;
}
