using TournamentManager.Application.Repositories;
using TournamentManager.Domain;

namespace TournamentManager.Application;

/// <summary>
/// Implementing class of the <see cref="IRoundService"/> interface
/// </summary>
public class RoundService : IRoundService
{
    private readonly IRepository<Round> _roundRepository;
    private readonly ITournamentService _tournamentService;

    /// <summary>
    /// Initializes a new instance of <see cref="RoundService"/>
    /// </summary>
    /// <param name="tournamentService">Service handling all <see cref="Tournament"/> actions.</param>
    /// <param name="roundRepository">Repository handling all <see cref="Round"/> actions for the datasource.</param>
    public RoundService(ITournamentService tournamentService, IRepository<Round> roundRepository)
    {
        _tournamentService = tournamentService;
        _roundRepository = roundRepository;
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        var origin = Get(id) ?? throw new NullReferenceException("Round not found");
        _roundRepository.Delete(origin);
    }

    /// <inheritdoc/>
    public Round Get(int id)
    {
        return _roundRepository.Get(id);
    }

    /// <inheritdoc/>
    public IEnumerable<Round> GetAll(int tournamentId)
    {
        var list = _roundRepository.GetAll()?.Where(u => u.Tournament.Id == tournamentId);
        if (list == null || !list.Any())
        {
            return null;
        }
        return list;
    }

    /// <inheritdoc/>
    public void Insert(int tournamentId, Round round)
    {
        ArgumentNullException.ThrowIfNull(round);

        if (round.Id != null) 
        {
            throw new ArgumentException("Id field has a value which is not allowed when adding a new instance");
        }

        round.Tournament = _tournamentService.Get(tournamentId) ?? throw new NullReferenceException("Tournament not found");
        round.CreatedDate = round.ModifiedDate = DateTime.UtcNow;

        _roundRepository.Insert(round);
    }

    /// <inheritdoc/>
    public Round Update(int id, Round round)
    {
        ArgumentNullException.ThrowIfNull(round);
        var origin = Get(id) ?? throw new NullReferenceException("Round not found");

        origin.Name = round.Name;
        origin.ModifiedDate = DateTime.UtcNow;

        _roundRepository.Update(origin);

        return origin;
    }
}
