using LunaSphere.Application.Users.DTOs;

namespace LunaSphere.Application.Auth.DTOs;

public record AuthDTO
(
    string AccessToken,
    string RefreshToken,
    UserDTO UserDetails
);