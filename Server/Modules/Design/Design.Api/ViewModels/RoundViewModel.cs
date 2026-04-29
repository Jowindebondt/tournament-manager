namespace Design.Api.ViewModels;

public class RoundViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? CompetitionType { get; set; }
    public string? KnockOutPhase { get; set; }
    public TournamentViewModel Tournament { get; set; } = null!;
}
