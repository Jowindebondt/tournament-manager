using TournamentManager.Domain;

namespace TournamentManager.Application;

public interface IPouleMemberService
{
    void Delete(int id);
    IEnumerable<PouleMember> GetAll(int pouleId);
    PouleMember Create(int pouleId, int memberId);
}
