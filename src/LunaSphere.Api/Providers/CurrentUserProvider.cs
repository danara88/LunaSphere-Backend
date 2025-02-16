using System.Security.Claims;

using LunaSphere.Application.Users.DTOs;
using LunaSphere.Application.Users.Interfaces;
using LunaSphere.Domain.Exceptions;

namespace LunaSphere.Api.Providers;

/// <summary>
/// Current user provider implementation
/// </summary>
public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccesor;

    public CurrentUserProvider(IHttpContextAccessor httpContextAccesor)
    {
        _httpContextAccesor = httpContextAccesor;
    }

    public CurrentUserDTO GetCurrentUser()
    {
        if (_httpContextAccesor.HttpContext is null)
        {
            throw new InternalServerException("Something went wrong.");
        }

        var id = GetClaimValues("id")
            .Select(int.Parse)
            .First();

        var roles = GetClaimValues(ClaimTypes.Role);

        return new CurrentUserDTO(
            Id: id,
            Roles: roles
        );
    }

    private IReadOnlyList<string> GetClaimValues(string claimType)
    {
        return _httpContextAccesor.HttpContext!.User.Claims
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .ToList();
    }
}