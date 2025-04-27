using LunaSphere.Domain.Common.Interfaces;

namespace LunaSphere.Domain.Users.Events;

/// <summary>
/// Event associated when a user resent a verification code
/// </summary>
public record UserResentVerificationCodeEvent(int userId) : IDomainEvent;