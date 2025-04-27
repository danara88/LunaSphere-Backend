using LunaSphere.Domain.Common.Interfaces;

namespace LunaSphere.Domain.Users.Events;

/// <summary>
/// Event associated when a new user is registered in the system
/// </summary>
public record UserRegisteredEvent(int userId) : IDomainEvent;