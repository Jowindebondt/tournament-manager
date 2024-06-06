using TournamentManager.Domain;

namespace TournamentManager.Application;

/// <summary>
/// Interface defining the service for handling actions for the <see cref="Poule"/> model.
/// </summary>
public interface IPouleService
{
    /// <summary>
    /// Add the provided members to the current <see cref="Poule"/> as individual <see cref="Player"/>s
    /// <summary>
    /// <paramref name="id"/>The identifier of the <see cref="Poule"/></param>
    /// <paramref name="memberIds"/>The identifiers of the <see cref="Member"/>s to add</param>
    void AddMembers(int id, IEnumerable<int> memberIds);
    /// <summary>
    /// Add the provided members to the current <see cref="Poule"/> as a single <see cref="Player"/>
    /// <summary>
    /// <paramref name="id"/>The identifier of the <see cref="Poule"/></param>
    /// <paramref name="memberIds"/>The identifiers of the <see cref="Member"/>s to add</param>
    void AddMembersAsTeam(int id, IEnumerable<int> memberIds);
    /// <summary>
    /// Deletes an existing <see cref="Poule"/> instance.
    /// </summary>
    /// <param name="id">The identifier of the <see cref="Poule"/></param>
    void Delete(int id);
    /// <summary>
    /// Gets a single <see cref="Poule"/> based on its identifier.
    /// </summary>
    /// <param name="id">The identifier of a <see cref="Poule"/></param>
    /// <returns>The requested <see cref="Poule"/></returns>
    Poule Get(int id);
    /// <summary>
    /// Gets a list of available <see cref="Poule"/>s for the specified <see cref="Round"/>.
    /// </summary>
    /// <param name="parentId">Id of the <see cref="Round"/></param>
    /// <returns>List of <see cref="Poule"/>s for the specified <see cref="Round"/></returns>
    IEnumerable<Poule> GetAll(int parentId);
    /// <summary>
    /// Create a new instance of <see cref="Poule"/>
    /// </summary>
    /// <param name="entity">The new values for <see cref="Poule"/></param>
    void Insert(Poule entity);
    /// <summary>
    /// Updates an existing <see cref="Poule"/> instance.
    /// </summary>
    /// <param name="id">The identifier of the <see cref="Poule"/></param>
    /// <param name="entity">The <see cref="Poule"/> changes.</param>
    /// <returns>The updated <see cref="Poule"/></returns>
    Poule Update(int id, Poule entity);
}
