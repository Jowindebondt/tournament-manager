namespace Design.Application.DTOs;

public class RoundDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? CompetitionType { get; set; }
    public string? KnockOutPhase { get; set; }
    public TournamentDto Tournament { get; set; } = null!;
}
