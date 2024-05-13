using TournamentManager.Domain;

namespace TournamentManager.Application;

/// <summary>
/// Interface defining the service for handling actions for the <see cref="Member"/> model.
/// </summary>
public interface IMemberService
{
    /// <summary>
    /// Deletes an existing <see cref="Member"/> instance.
    /// </summary>
    /// <param name="id">The identifier of the <see cref="Member"/></param>
    void Delete(int id);
    /// <summary>
    /// Gets a single <see cref="Member"/> based on its identifier.
    /// </summary>
    /// <param name="id">The identifier of a <see cref="Member"/></param>
    /// <returns>The requested <see cref="Member"/></returns>
    Member Get(int id);
    /// <summary>
    /// Gets a list of available <see cref="Member"/>s for the specified <see cref="Tournament"/>.
    /// </summary>
    /// <param name="parentId">Id of the <see cref="Tournament"/></param>
    /// <returns>List of <see cref="Member"/>s for the specified <see cref="Tournament"/></returns>
    IEnumerable<Member> GetAll(int parentId);
    /// <summary>
    /// Create a new instance of <see cref="Member"/>
    /// </summary>
    /// <param name="entity">The new values for <see cref="Member"/></param>
    void Insert(Member entity);
    /// <summary>
    /// Updates an existing <see cref="Member"/> instance.
    /// </summary>
    /// <param name="id">The identifier of the <see cref="Member"/></param>
    /// <param name="entity">The <see cref="Member"/> changes.</param>
    /// <returns>The updated <see cref="Member"/></returns>
    Member Update(int id, Member entity);
}
