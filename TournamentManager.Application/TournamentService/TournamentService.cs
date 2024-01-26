using TournamentManager.Application.Repositories;
using TournamentManager.Domain;

namespace TournamentManager.Application;

public class TournamentService : ITournamentService
{
    private readonly IRepository<Tournament> _tournamentRepository;

    public TournamentService(IRepository<Tournament> tournamentRepository) {
        _tournamentRepository = tournamentRepository;
    }

    public Tournament Get(int id)
    {
        return _tournamentRepository.Get(id);
    }

    public IEnumerable<Tournament> GetAll()
    {
        var list = _tournamentRepository.GetAll();
        if (list == null || !list.Any()){
            return null;
        }
        return list;
    }

    public void Insert(Tournament tournament)
    {
        ArgumentNullException.ThrowIfNull(tournament);

        if (tournament.Id != null) {
            throw new ArgumentException("Id field has a value which is not allowed when adding a new instance");
        }

        tournament.CreatedDate = DateTime.UtcNow;
        tournament.ModifiedDate = DateTime.UtcNow;

        _tournamentRepository.Insert(tournament);
    }
}
