namespace LunaSphere.Application.Auth.DTOs;

/// <summary>
/// Represents the DTO response when a user is verified with 
/// google provider services for authentication.
/// </summary>
public record GoogleAuthRespDTO
(
    string Name,
    string FamilyName,
    string Email,
    string Picture
);
