namespace TournamentManager.Domain;

/// <summary>
/// Class defining the base model
/// </summary>
public class BaseEntity
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public int? Id { get; set; }
    /// <summary>
    /// Date of creation
    /// </summary>
    public DateTime? CreatedDate { get; set; }
    /// <summary>
    /// Date of last modification
    /// </summary>
    public DateTime? ModifiedDate { get; set; }
}
