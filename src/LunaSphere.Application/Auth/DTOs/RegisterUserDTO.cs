namespace LunaSphere.Application.Auth.DTOs;

public record RegisterUserDTO
(
    string VerificationToken,
    string VerificationTokenExpires
);