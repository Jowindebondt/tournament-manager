using TournamentManager.Domain;

namespace TournamentManager.Application.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    void Delete(T origin);
    T Get(int id);
    IEnumerable<T> GetAll();
    void Insert(T entity);
    void Update(T origin);
}
