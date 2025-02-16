namespace LunaSphere.Application.Auth.DTOs;

public record RefreshTokenDTO
(
    string AccessToken,
    string RefreshToken
);