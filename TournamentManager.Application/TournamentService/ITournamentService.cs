using TournamentManager.Domain;

namespace TournamentManager.Application;

public interface ITournamentService
{
    IEnumerable<Tournament> GetAll();
    Tournament Get(int id);
    void Insert(Tournament tournament);
    Tournament Update(int id, Tournament tournament);
    void Delete(int id);
}
