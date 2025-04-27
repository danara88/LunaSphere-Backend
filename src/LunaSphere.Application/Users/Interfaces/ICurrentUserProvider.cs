using ErrorOr;
using LunaSphere.Application.Users.DTOs;

namespace LunaSphere.Application.Users.Interfaces;

public interface ICurrentUserProvider
{
    ErrorOr<CurrentUserDTO> GetCurrentUser();
}