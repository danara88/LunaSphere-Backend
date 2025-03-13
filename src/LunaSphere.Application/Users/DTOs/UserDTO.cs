using LunaSphere.Domain.Users.Enums;

namespace LunaSphere.Application.Users.DTOs;

public record UserDTO
(
    int Id,
    string? FirstName,
    string? LastName,
    string Email,
    RoleType Role,
    bool IsGoogle,
    DateTime? VerifiedAt,
    DateTime LastLogin
);