namespace Design.Api.ViewModels;

public class CreateRoundViewModel
{
    public string Name { get; set; } = string.Empty;
    public Guid TournamentId { get; set; }
}
