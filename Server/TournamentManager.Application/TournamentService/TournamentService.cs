using TournamentManager.Application.Repositories;
using TournamentManager.Domain;

namespace TournamentManager.Application;

/// <summary>
/// Implementing class of the <see cref="ITournamentService"/> interface
/// </summary>
public class TournamentService : ITournamentService
{
    private readonly ICrudService<Tournament> _crudService;
    private readonly ITournamentRepository _tournamentRepository;
    private readonly ICrudService<TournamentSettings> _settingsCrudService;
    private readonly SportServiceResolver _sportServiceResolver;

    /// <summary>
    /// Initializes a new instance of <see cref="TournamentService"/>
    /// </summary>
    /// <param name="crudService">Service for handling CRUD actions for the <see cref="Tournament"/> model.</param>
    public TournamentService(ICrudService<Tournament> crudService, ITournamentRepository tournamentRepository, ICrudService<TournamentSettings> settingsCrudService, SportServiceResolver sportServiceResolver){
        _crudService = crudService;
        _tournamentRepository = tournamentRepository;
        _settingsCrudService = settingsCrudService;
        _sportServiceResolver = sportServiceResolver;
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        _crudService.Delete(id);
    }

    /// <inheritdoc/>
    public void Generate(int id)
    {
        var tournament = _tournamentRepository.GetWithAllReferences(id) ?? throw new ArgumentException("Tournament not found");
        _sportServiceResolver(tournament.Sport).Generate(tournament);
    }

    /// <inheritdoc/>
    public Tournament Get(int id)
    {
        return _crudService.Get(id);
    }

    /// <inheritdoc/>
    public IEnumerable<Tournament> GetAll()
    {
        return _crudService.GetAll();
    }

    /// <inheritdoc/>
    public TournamentSettings GetSettings(int id)
    {
        var tournament = GetWithSettings(id) ?? throw new ArgumentException("Tournament not found");
        if (tournament.Settings == null)
        {
            throw new ArgumentException("Settings not found");
        }
        return tournament.Settings;
    }

    /// <inheritdoc/>
    public void Insert(Tournament entity)
    {
        _crudService.Insert(entity);
    }

    /// <inheritdoc/>
    public void SetSettings(TournamentSettings settings)
    {
        var tournament = GetWithSettings(settings.TournamentId.Value) ?? throw new ArgumentException("Tournament not found");
        if (tournament.Settings != null)
        {
            // Remove old settings
            _settingsCrudService.Delete(tournament.Settings.Id.Value);
        }
        _settingsCrudService.Insert(settings);
    }

    /// <inheritdoc/>
    public Tournament Update(int id, Tournament entity)
    {
        return _crudService.Update(id, entity, origin => {
            origin.Name = entity.Name;
        });
    }

    private Tournament GetWithSettings(int id)
    {
        return _tournamentRepository.GetWithSettings(id);
    }
}
