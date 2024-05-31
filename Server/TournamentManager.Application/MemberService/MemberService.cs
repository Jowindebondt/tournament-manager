using TournamentManager.Application.Repositories;
using TournamentManager.Domain;

namespace TournamentManager.Application;

/// <summary>
/// Implementing class of the <see cref="IMemberService"/> interface
/// </summary>
public class MemberService : IMemberService
{
    private readonly ICrudService<Member> _crudService;

    /// <summary>
    /// Initializes a new instance of <see cref="MemberService"/>
    /// </summary>
    /// <param name="crudService">Service for handling CRUD actions for the <see cref="Member"/> model.</param>
    public MemberService(ICrudService<Member> crudService)
    {
        _crudService = crudService;
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        _crudService.Delete(id);
    }

    /// <inheritdoc/>
    public Member Get(int id)
    {
        return _crudService.Get(id);
    }

    /// <inheritdoc/>
    public IEnumerable<Member> GetAll(int parentId)
    {
        return _crudService.GetAll(u => u.TournamentId == parentId);
    }

    /// <inheritdoc/>
    public void Insert(Member entity)
    {
        _crudService.Insert(entity);
    }

    /// <inheritdoc/>
    public Member Update(int id, Member entity)
    {
        return _crudService.Update(id, entity, (origin) => {
            origin.Name = entity.Name;
        });
    }
}
