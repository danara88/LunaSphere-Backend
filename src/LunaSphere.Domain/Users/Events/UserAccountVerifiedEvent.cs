using LunaSphere.Domain.Common.Interfaces;

namespace LunaSphere.Domain.Users.Events;

/// <summary>
/// Event associated when a user account was verified in the system
/// </summary>
public record UserAccountVerifiedEvent(int userId, string refreshToken) : IDomainEvent;