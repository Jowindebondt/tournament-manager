using TournamentManager.Domain;

namespace TournamentManager.Application;

public class PoulePlayerService : IPoulePlayerService
{
    private readonly ICrudService<PoulePlayer> _crudService;
    private readonly IPouleService _pouleService;
    private readonly IPlayerService _playerService;

    public PoulePlayerService(ICrudService<PoulePlayer> crudService, IPouleService pouleService, IPlayerService playerService)
    {
        _crudService = crudService;
        _pouleService = pouleService;
        _playerService = playerService;
    }

    public PoulePlayer Create(int pouleId, int playerId)
    {
        var pouleMember = new PoulePlayer
        {
            Poule = _pouleService.Get(pouleId) ?? throw new NullReferenceException($"{nameof(Poule)} not found"),
            Player = _playerService.Get(playerId) ?? throw new NullReferenceException($"{nameof(Member)} not found")
        };
        _crudService.Insert(pouleMember);
        return pouleMember;
    }

    public void Delete(int id)
    {
        _crudService.Delete(id);
    }

    public IEnumerable<PoulePlayer> GetAll(int pouleId)
    {
        return _crudService.GetAll(u => u.Poule.Id == pouleId);
    }
}
