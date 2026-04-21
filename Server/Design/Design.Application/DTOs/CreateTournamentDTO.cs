using Design.Domain.Enums;

namespace Design.Application.DTOs;

public class CreateTournamentDTO
{
    public string Name { get; set; }
    public Sport Sport { get; set; }
}
