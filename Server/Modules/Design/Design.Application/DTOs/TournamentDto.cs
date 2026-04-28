using Design.Domain.Enums;

namespace Design.Application.DTOs;

public class TournamentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Sport Sport { get; set; }
}
