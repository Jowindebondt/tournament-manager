namespace Design.Application.DTOs;

public class CreatePouleDto
{
    public string Name { get; set; }
    public int TotalPlayers { get; set; }
    public Guid RoundId { get; set; }
}
