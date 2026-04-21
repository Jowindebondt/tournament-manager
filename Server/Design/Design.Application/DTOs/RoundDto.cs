namespace Design.Application.DTOs;

public class RoundDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public TournamentDTO Tournament { get; set; }
}
