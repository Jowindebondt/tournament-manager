using TournamentManager.Domain;

namespace TournamentManager.Application;

public interface IPoulePlayerService
{
    void Delete(int id);
    IEnumerable<PoulePlayer> GetAll(int pouleId);
    PoulePlayer Create(int pouleId, int memberId);
}
