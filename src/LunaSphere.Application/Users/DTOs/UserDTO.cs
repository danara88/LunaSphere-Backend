namespace LunaSphere.Application.Users.DTOs;

public record UserDTO
(
    string? FirstName,
    string? LastName,
    string Email,
    bool IsGoogle,
    DateTime LastLogin
);