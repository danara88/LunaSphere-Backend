namespace LunaSphere.Application.Auth.DTOs;

public record CreateUserAccountDTO
(
    string Email,
    string Password
);