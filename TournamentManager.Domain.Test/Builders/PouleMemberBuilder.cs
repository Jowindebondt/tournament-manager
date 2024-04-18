namespace TournamentManager.Domain.Test;

public class PouleMemberBuilder
{
    public static PoulePlayer GetSinglePouleMember(int id, int pouleId, int memberId)
    {
        return new PoulePlayer
        {
            Id = id * -1,
            Poule = PouleBuilder.GetSinglePoule(pouleId),
            Player = PlayerBuilder.GetSinglePlayer(memberId),
            CreatedDate = new DateTime(2024, 1, 1),
            ModifiedDate = new DateTime(2024, 1, 1),
        };
    }
}
