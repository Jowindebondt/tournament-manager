using Design.Domain.Enums;

namespace Design.Application.DTOs;

public class CreateTournamentDto
{
    public string Name { get; set; } = string.Empty;
    public Sport Sport { get; set; }
}
