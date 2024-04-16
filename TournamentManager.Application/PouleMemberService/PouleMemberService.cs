using TournamentManager.Domain;

namespace TournamentManager.Application;

public class PouleMemberService : IPouleMemberService
{
    private readonly ICrudService<PouleMember> _crudService;
    private readonly IPouleService _pouleService;
    private readonly IMemberService _memberService;

    public PouleMemberService(ICrudService<PouleMember> crudService, IPouleService pouleService, IMemberService memberService)
    {
        _crudService = crudService;
        _pouleService = pouleService;
        _memberService = memberService;
    }

    public PouleMember Create(int pouleId, int memberId)
    {
        var pouleMember = new PouleMember
        {
            Poule = _pouleService.Get(pouleId) ?? throw new NullReferenceException($"{nameof(Poule)} not found"),
            Member = _memberService.Get(memberId) ?? throw new NullReferenceException($"{nameof(Member)} not found")
        };
        _crudService.Insert(pouleMember);
        return pouleMember;
    }

    public void Delete(int id)
    {
        _crudService.Delete(id);
    }

    public IEnumerable<PouleMember> GetAll(int pouleId)
    {
        return _crudService.GetAll(u => u.Poule.Id == pouleId);
    }
}
