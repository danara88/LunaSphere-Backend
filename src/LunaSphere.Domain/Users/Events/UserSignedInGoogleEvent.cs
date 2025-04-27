using LunaSphere.Domain.Common.Interfaces;

namespace LunaSphere.Domain.Users.Events;

/// <summary>
/// Event associated when a user signed in using Google
/// </summary>
public record UserSignedInGoogleEvent(int userId, string refreshToken) : IDomainEvent;