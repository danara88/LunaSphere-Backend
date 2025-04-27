using LunaSphere.Domain.Common.Interfaces;

namespace LunaSphere.Domain.Common;

/// <summary>
/// Represents the base for all domain entities
/// </summary>
public abstract class Entity
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

    /// <summary>
    /// Store the domain events
    /// </summary>
    public readonly List<IDomainEvent> _domainEvents = new();

    public List<IDomainEvent> PopDomainEvents()
    {
        var copy = _domainEvents.ToList();

        _domainEvents.Clear();

        return copy;
    }
    
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override bool Equals(object? other)
    {
        if(other is null || other.GetType() != GetType())
        {
            return false;
        }
        return ((Entity)other).Id == Id;
    }
}