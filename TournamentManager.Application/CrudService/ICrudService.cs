using TournamentManager.Domain;

namespace TournamentManager.Application;

/// <summary>
/// Interface defining the service for handling CRUD actions for the <see cref="T"/> model.
/// </summary>
/// <typeparam name="T">Type of the <see cref="BaseEntity"/> model</typeparam>
public interface ICrudService<T>
    where T : BaseEntity
{
    /// <summary>
    /// Gets a list of available <see cref="T"/>s for the specified <see cref="U"/>.
    /// </summary>
    /// <returns>List of <see cref="T"/>s for the specified <see cref="U"/></returns>
    IEnumerable<T> GetAll();
    /// <summary>
    /// Gets a list of available <see cref="T"/>s for the specified <see cref="U"/>.
    /// </summary>
    /// <param name="predicate">The filter to use for collecting the list</param>
    /// <returns>List of <see cref="T"/>s for the specified <see cref="U"/></returns>
    IEnumerable<T> GetAll(Func<T, bool> predicate);
    /// <summary>
    /// Gets a single <see cref="T"/> based on its identifier.
    /// </summary>
    /// <param name="id">The identifier of a <see cref="T"/></param>
    /// <returns>The requested <see cref="T"/></returns>
    T Get(int id);
    /// <summary>
    /// Create a new instance of <see cref="T"/>
    /// </summary>
    /// <param name="entity">The new values for <see cref="T"/></param>
    /// <param name="setTypeSpecifics">Set type specific system values</param>
    void Insert(T entity, Action setTypeSpecifics = null);
    /// <summary>
    /// Updates an existing <see cref="T"/> instance.
    /// </summary>
    /// <param name="id">The identifier of the <see cref="T"/></param>
    /// <param name="entity">The <see cref="T"/> changes.</param>
    /// <param name="updateTypeSpecifics">Update the type specific values for the provided existing instance.</param>
    /// <returns>The updated <see cref="T"/></returns>
    T Update(int id, T entity, Action<T> updateTypeSpecifics);
    /// <summary>
    /// Deletes an existing <see cref="T"/> instance.
    /// </summary>
    /// <param name="id">The identifier of the <see cref="T"/></param>
    void Delete(int id);
}