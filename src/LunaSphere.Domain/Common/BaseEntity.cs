namespace LunaSphere.Domain.Common;

/// <summary>
/// Represents the base for all domain entities
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Entity primary key
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Indicates whether the entity is active or soft-deleted.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Creation date of the domain entity
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Updated date of the domain entity
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}