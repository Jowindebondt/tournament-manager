using TournamentManager.Application.Repositories;
using TournamentManager.Domain;

namespace TournamentManager.Application;

/// <summary>
/// Implementing class of the <see cref="ITournamentService"/> interface
/// </summary>
public class TournamentService : ITournamentService
{
    private readonly IRepository<Tournament> _tournamentRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="TournamentService"/>
    /// </summary>
    /// <param name="tournamentRepository">Repository handling all <see cref="Tournament"/> actions for the datasource.</param>
    public TournamentService(IRepository<Tournament> tournamentRepository) {
        _tournamentRepository = tournamentRepository;
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        var origin = Get(id) ?? throw new NullReferenceException("Tournament not found");
        _tournamentRepository.Delete(origin);
    }

    /// <inheritdoc/>
    public Tournament Get(int id)
    {
        return _tournamentRepository.Get(id);
    }

    /// <inheritdoc/>
    public IEnumerable<Tournament> GetAll()
    {
        var list = _tournamentRepository.GetAll();
        if (list == null || !list.Any())
        {
            return null;
        }
        return list;
    }

    /// <inheritdoc/>
    public void Insert(Tournament tournament)
    {
        ArgumentNullException.ThrowIfNull(tournament);

        if (tournament.Id != null) 
        {
            throw new ArgumentException("Id field has a value which is not allowed when adding a new instance");
        }

        tournament.CreatedDate = tournament.ModifiedDate = DateTime.UtcNow;

        _tournamentRepository.Insert(tournament);
    }

    /// <inheritdoc/>
    public Tournament Update(int id, Tournament tournament)
    {
        ArgumentNullException.ThrowIfNull(tournament);
        var origin = Get(id) ?? throw new NullReferenceException("Tournament not found");

        origin.Name = tournament.Name;
        origin.ModifiedDate = DateTime.UtcNow;

        _tournamentRepository.Update(origin);

        return origin;
    }
}
