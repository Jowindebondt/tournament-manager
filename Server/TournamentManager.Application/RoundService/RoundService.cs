using TournamentManager.Application.Repositories;
using TournamentManager.Domain;

namespace TournamentManager.Application;

/// <summary>
/// Implementing class of the <see cref="IRoundService"/> interface
/// </summary>
public class RoundService : IRoundService
{
    private readonly ICrudService<Round> _crudService;
    private readonly ICrudService<RoundSettings> _settingsCrudService;

    /// <summary>
    /// Initializes a new instance of <see cref="RoundService"/>
    /// </summary>
    /// <param name="tournamentService">Service handling all <see cref="Tournament"/> actions.</param>
    /// <param name="crudService">Service for handling CRUD actions for the <see cref="Round"/> model.</param>
    public RoundService(ICrudService<Round> crudService, ICrudService<RoundSettings> settingsCrudService)
    {
        _crudService = crudService;
        _settingsCrudService = settingsCrudService;
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        _crudService.Delete(id);
    }

    /// <inheritdoc/>
    public Round Get(int id)
    {
        return _crudService.Get(id);
    }

    /// <inheritdoc/>
    public IEnumerable<Round> GetAll(int parentId)
    {
        return _crudService.GetAll(u => u.TournamentId == parentId);
    }

    /// <inheritdoc/>
    public void Insert(Round entity)
    {
        _crudService.Insert(entity);
    }

    /// <inheritdoc/>
    public void SetSettings(RoundSettings settings)
    {
        var _ = _crudService.Get(settings.RoundId) ?? throw new ArgumentException("Round not found");
        _settingsCrudService.Insert(settings);
    }

    /// <inheritdoc/>
    public Round Update(int id, Round entity)
    {
        return _crudService.Update(id, entity, (origin) => {
            origin.Name = entity.Name;
        });
    }
}
