using System.Linq.Expressions;
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
    protected ApplicationDbContext ApplicationDbContext { get; }
    protected DbSet<T> Entities { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="Repository{T}"/>
    /// </summary>
    /// <param name="applicationDbContext">The database context</param>
    public Repository(ApplicationDbContext applicationDbContext) {
        ApplicationDbContext = applicationDbContext;
        Entities = ApplicationDbContext.Set<T>();
    }

    /// <inheritdoc/>
    public void Delete(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        Entities.Remove(entity);
        ApplicationDbContext.SaveChanges();
    }

    /// <inheritdoc/>
    public T Get(int id)
    {
        return Entities.Find(id);
    }

    /// <inheritdoc/>
    public IEnumerable<T> GetAll()
    {
        return Entities.AsQueryable();
    }

    /// <inheritdoc/>
    public void Insert(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        Entities.Add(entity);
        ApplicationDbContext.SaveChanges();
    }

    /// <inheritdoc/>
    public void Update(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        Entities.Update(entity);
        ApplicationDbContext.SaveChanges();
    }
}
