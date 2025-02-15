namespace LunaSphere.Application.Users.DTOs;

/// <summary>
/// Current user details
/// </summary>
public record CurrentUserDTO
(
    int Id,
    IReadOnlyList<string> Roles
);