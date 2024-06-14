using TournamentManager.Application.Repositories;
using TournamentManager.Domain;

namespace TournamentManager.Application;

/// <summary>
/// Implementing class of the <see cref="ITournamentService"/> interface
/// </summary>
public class TournamentService : ITournamentService
{
    private readonly ICrudService<Tournament> _crudService;
    private readonly ICrudService<TournamentSettings> _settingsCrudService;
    private readonly SportServiceResolver _sportServiceResolver;

    /// <summary>
    /// Initializes a new instance of <see cref="TournamentService"/>
    /// </summary>
    /// <param name="crudService">Service for handling CRUD actions for the <see cref="Tournament"/> model.</param>
    public TournamentService(ICrudService<Tournament> crudService, ICrudService<TournamentSettings> settingsCrudService, SportServiceResolver sportServiceResolver){
        _crudService = crudService;
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
        var tournament = _crudService.Get(id) ?? throw new ArgumentException("Tournament not found");
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
    public void Insert(Tournament entity)
    {
        _crudService.Insert(entity);
    }

    /// <inheritdoc/>
    public void SetSettings(TournamentSettings settings)
    {
        var _ = _crudService.Get(settings.TournamentId) ?? throw new ArgumentException("Tournament not found");
        _settingsCrudService.Insert(settings);
    }

    /// <inheritdoc/>
    public Tournament Update(int id, Tournament entity)
    {
        return _crudService.Update(id, entity, origin => {
            origin.Name = entity.Name;
        });
    }
}
