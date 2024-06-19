using TournamentManager.Application.Repositories;
using TournamentManager.Domain;

namespace TournamentManager.Application;

public class PouleService : IPouleService
{
    private readonly ICrudService<Poule> _crudService;
    private readonly IMemberService _memberService;
    private readonly IPlayerService _playerService;
    private readonly IPouleRepository _pouleRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="PouleService"/>
    /// </summary>
    /// <param name="crudService">Service for handling CRUD actions for the <see cref="Poule"/> model.</param>
    public PouleService(ICrudService<Poule> crudService, IMemberService memberService, IPlayerService playerService, IPouleRepository pouleRepository) 
    {
        _crudService = crudService;
        _memberService = memberService;
        _playerService = playerService;
        _pouleRepository = pouleRepository;
    }

    /// <inheritdoc/>
    public void AddMembers(int id, IEnumerable<int> memberIds)
    {
        var poule = _pouleRepository.GetWithAncestors(id) ?? throw new ArgumentException("Poule not found");
        var tournamentId = poule.Round.TournamentId;

        var membersNotFound = new List<int>();
        foreach (var memberId in memberIds)
        {
            var member = _memberService.Get(memberId);
            if (member == null || member.TournamentId != tournamentId)
            {
                membersNotFound.Add(memberId);
            }
            else 
            {
                var player = new Player
                {
                };
                _playerService.Insert(player);

                member.PlayerId = player.Id.Value;
                _memberService.Update(memberId, member);

                poule.Players.Add(player);
                _crudService.Update(poule.Id.Value, poule, _ => {});
            }
        }

        if (membersNotFound.Count > 0)
        {
            throw new ArgumentException($"The following members were not found in current tournament: {string.Join(',', membersNotFound)}");
        }
    }

    /// <inheritdoc/>
    public void AddMembersAsTeam(int id, IEnumerable<int> memberIds)
    {
        var poule = _pouleRepository.GetWithAncestors(id) ?? throw new ArgumentException("Poule not found");
        var tournamentId = poule.Round.TournamentId;

        var player = new Player();
        _playerService.Insert(player);

        var membersNotFound = new List<int>();
        foreach (var memberId in memberIds)
        {
            var member = _memberService.Get(memberId);
            if (member == null || member.TournamentId != tournamentId)
            {
                membersNotFound.Add(memberId);
            }
            else 
            {
                member.PlayerId = player.Id.Value;
                _memberService.Update(memberId, member);
            }
        }

        if (membersNotFound.Count == memberIds.Count())
        {
            // if all members aren't found, then the newly created player can be removed because it won't have any references
            _playerService.Delete(player.Id.Value);
        }
        else
        {
            poule.Players.Add(player);
            _crudService.Update(poule.Id.Value, poule, _ => {});
        }
        if (membersNotFound.Count > 0)
        {
            throw new ArgumentException($"The following members were not found in current tournament: {string.Join(',', membersNotFound)}");
        }
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        _crudService.Delete(id);
    }

    /// <inheritdoc/>
    public Poule Get(int id)
    {
        return _crudService.Get(id);
    }

    /// <inheritdoc/>
    public IEnumerable<Poule> GetAll(int parentId)
    {
        return _crudService.GetAll(u => u.RoundId == parentId);
    }

    /// <inheritdoc/>
    public void Insert(Poule entity)
    {
        _crudService.Insert(entity);
    }

    /// <inheritdoc/>
    public Poule Update(int id, Poule entity)
    {
        return _crudService.Update(id, entity, (origin) => {
            origin.Name = entity.Name;
        });
    }
}
