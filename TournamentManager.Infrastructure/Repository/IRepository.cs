using TournamentManager.Domain;

namespace TournamentManager.Infrastructure;

public interface IRepository<T> where T : BaseEntity
{
    T Get(int id);
    IEnumerable<T> GetAll();
    void Insert(T entity);
}
