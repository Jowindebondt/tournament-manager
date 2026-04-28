namespace Design.Application.DTOs;

public class CreateRoundDto
{
    public string Name { get; set; } = string.Empty;
    public Guid TournamentId { get; set; }
}
