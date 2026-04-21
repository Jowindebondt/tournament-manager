namespace Design.Application.DTOs;

public class PouleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int TotalPlayers { get; set; }
    public RoundDto Round { get; set; }
}
