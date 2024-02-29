using TournamentManager.Application.Repositories;
using TournamentManager.Domain;

namespace TournamentManager.Application;

/// <summary>
/// Implementing class of the <see cref="IMemberService"/> interface
/// </summary>
public class MemberService : IMemberService
{
    private readonly ITournamentService _tournamentService;
    private readonly ICrudService<Member> _crudService;

    /// <summary>
    /// Initializes a new instance of <see cref="MemberService"/>
    /// </summary>
    /// <param name="tournamentService">Service handling all <see cref="Tournament"/> actions.</param>
    /// <param name="crudService">Service for handling CRUD actions for the <see cref="Member"/> model.</param>
    public MemberService(ITournamentService tournamentService, ICrudService<Member> crudService)
    {
        _tournamentService = tournamentService;
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
        return _crudService.GetAll(u => u.Tournament.Id == parentId);
    }

    /// <inheritdoc/>
    public void Insert(int parentId, Member entity)
    {
        _crudService.Insert(entity, () => {
            var tournament = _tournamentService.Get(parentId) ?? throw new NullReferenceException($"{nameof(Tournament)} not found"); 
            entity.Tournament = tournament;
        });
    }

    /// <inheritdoc/>
    public Member Update(int id, Member entity)
    {
        return _crudService.Update(id, entity, (origin) => {
            origin.Name = entity.Name;
        });
    }
}
