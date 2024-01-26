using Microsoft.EntityFrameworkCore;
using TournamentManager.Application.Repositories;
using TournamentManager.Domain;

namespace TournamentManager.Infrastructure;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly ApplicationDbContext _applicationDbContext;
    private DbSet<T> _entities;

    public Repository(ApplicationDbContext applicationDbContext) {
        _applicationDbContext = applicationDbContext;
        _entities = _applicationDbContext.Set<T>();
    }

    public T Get(int id)
    {
        return _entities.SingleOrDefault(u => u.Id == id);
    }

    public IEnumerable<T> GetAll()
    {
        return _entities.AsEnumerable();
    }

    public void Insert(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _entities.Add(entity);
        _applicationDbContext.SaveChanges();
    }
}
