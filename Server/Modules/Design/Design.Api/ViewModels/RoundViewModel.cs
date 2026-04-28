namespace Design.Api.ViewModels;

public class RoundViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TournamentViewModel Tournament { get; set; } = null!;
}
