using TournamentManager.Application.Repositories;
using TournamentManager.Domain;

namespace TournamentManager.Application;

/// <summary>
/// Implementing class of the <see cref="ICrudService{T}"/> interface.
/// </summary>
/// <typeparam name="T">Type of the <see cref="BaseEntity"/> model</typeparam>
public class CrudService<T> : ICrudService<T>
    where T : BaseEntity
{
    private readonly IRepository<T> _repository;

    /// <summary>
    /// Initializes a new instance of <see cref="CrudService{T}"/>
    /// </summary>
    /// <param name="repository">Repository handling all <see cref="T"/> actions for the datasource.</param>
    public CrudService(IRepository<T> repository)
    {
        _repository = repository;
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        var origin = Get(id) ?? throw new NullReferenceException($"{typeof(T).Name} not found");
        _repository.Delete(origin);
    }

    /// <inheritdoc/>
    public T Get(int id)
    {
        return _repository.Get(id);
    }

    /// <inheritdoc/>
    public IEnumerable<T> GetAll()
    {
        var list = _repository.GetAll();
        if (list == null || !list.Any())
        {
            return null;
        }
        return list;
    }

    /// <inheritdoc/>
    public IEnumerable<T> GetAll(Func<T, bool> predicate)
    {
        var list = _repository.GetAll().Where(predicate);
        if (list == null || !list.Any())
        {
            return null;
        }
        return list;
    }

    /// <inheritdoc/>
    public void Insert(T entity, Action setTypeSpecifics = null)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (entity.Id != null) 
        {
            throw new ArgumentException("Id field has a value which is not allowed when adding a new instance");
        }

        setTypeSpecifics?.Invoke();
        entity.CreatedDate = entity.ModifiedDate = DateTime.UtcNow;

        _repository.Insert(entity);
    }

    /// <inheritdoc/>
    public T Update(int id, T entity, Action<T> updateTypeSpecifics)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var origin = Get(id) ?? throw new NullReferenceException($"{typeof(T).Name} not found");

        updateTypeSpecifics.Invoke(origin);
        origin.ModifiedDate = DateTime.UtcNow;

        _repository.Update(origin);

        return origin;
    }
}
