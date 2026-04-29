namespace Competition.Application.DTOs;

public class MatchDto
{
    public Guid Id { get; set; }
    public short Player1Index { get; set; }
    public short Player2Index { get; set; }
    public short? Player1Score { get; set; }
    public short? Player2Score { get; set; }
    public Guid PouleId { get; set; }
}
