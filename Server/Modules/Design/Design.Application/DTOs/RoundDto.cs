namespace Design.Application.DTOs;

public class RoundDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Type { get; set; }
    public TournamentDto Tournament { get; set; } = null!;
}
