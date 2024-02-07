using TournamentManager.Domain;

namespace TournamentManager.Application.Repositories;

/// <summary>
/// Interface defining the actions available for the datasource
/// </summary>
/// <typeparam name="T">Type of the <see cref="BaseEntity"/> model</typeparam>
public interface IRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Deletes an instance from the datasource.
    /// </summary>
    /// <param name="entity">The original instance of <see cref="T"/></param>
    void Delete(T entity);
    /// <summary>
    /// Gets a single instance from the datasource.
    /// </summary>
    /// <param name="id">The identifier of the instance</param>
    /// <returns>The requested instance of <see cref="T"/></returns>
    T Get(int id);
    /// <summary>
    /// Gets all instances from the datasource.
    /// </summary>
    /// <returns>List of instances of <see cref="T"/></returns>
    IEnumerable<T> GetAll();
    /// <summary>
    /// Inserts an instance into the datasource.
    /// </summary>
    /// <param name="entity">The instance of <see cref="T"/> to insert</param>
    void Insert(T entity);
    /// <summary>
    /// Updates an instance in the datasource.
    /// </summary>
    /// <param name="entity">The updated instance of <see cref="T"/></param>
    void Update(T entity);
}
