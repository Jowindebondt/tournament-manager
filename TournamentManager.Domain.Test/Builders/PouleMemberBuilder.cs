namespace TournamentManager.Domain.Test;

public class PouleMemberBuilder
{
    public static PouleMember GetSinglePouleMember(int id, int pouleId, int memberId)
    {
        return new PouleMember
        {
            Id = id * -1,
            Poule = PouleBuilder.GetSinglePoule(pouleId),
            Member = MemberBuilder.GetSingleMember(memberId),
            CreatedDate = new DateTime(2024, 1, 1),
            ModifiedDate = new DateTime(2024, 1, 1),
        };
    }
}
