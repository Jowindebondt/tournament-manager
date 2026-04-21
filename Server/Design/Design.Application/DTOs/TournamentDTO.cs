using Design.Domain.Enums;

namespace Design.Application.DTOs;

public class TournamentDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Sport Sport { get; set; }
}
