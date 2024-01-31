using Microsoft.EntityFrameworkCore;
using TournamentManager.Application.Repositories;
using TournamentManager.Domain;

namespace TournamentManager.Infrastructure;

/// <summary>
/// Implementing class of the <see cref="IRepository{T}"/> interface.
/// </summary>
/// <typeparam name="T">Type of the <see cref="BaseEntity"/> model</typeparam>
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly ApplicationDbContext _applicationDbContext;
    private DbSet<T> _entities;

    /// <summary>
    /// Initializes a new instance of <see cref="Repository{T}"/>
    /// </summary>
    /// <param name="applicationDbContext">The database context</param>
    public Repository(ApplicationDbContext applicationDbContext) {
        _applicationDbContext = applicationDbContext;
        _entities = _applicationDbContext.Set<T>();
    }

    /// <inheritdoc/>
    public void Delete(T origin)
    {
        _entities.Remove(origin);
        _applicationDbContext.SaveChanges();
    }

    /// <inheritdoc/>
    public T Get(int id)
    {
        return _entities.SingleOrDefault(u => u.Id == id);
    }

    /// <inheritdoc/>
    public IEnumerable<T> GetAll()
    {
        return _entities.AsEnumerable();
    }

    /// <inheritdoc/>
    public void Insert(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _entities.Add(entity);
        _applicationDbContext.SaveChanges();
    }

    /// <inheritdoc/>
    public void Update(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _entities.Update(entity);
        _applicationDbContext.SaveChanges();
    }
}
