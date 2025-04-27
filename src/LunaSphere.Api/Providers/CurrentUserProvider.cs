using System.Security.Claims;
using ErrorOr;

using LunaSphere.Application.Users.DTOs;
using LunaSphere.Application.Users.Interfaces;
using LunaSphere.Domain.Users;

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

    public ErrorOr<CurrentUserDTO> GetCurrentUser()
    {
        if (_httpContextAccesor.HttpContext is null)
        {
            return AuthErrors.UserIsNotAuthenticated;
        }

        var idClaimValue = GetClaimValues("id");

        if(idClaimValue.IsError)
        {
            return AuthErrors.UserIsNotAuthenticated;
        }

        var id = idClaimValue.Value
            .Select(int.Parse)
            .First();

        var rolesClaimValue = GetClaimValues(ClaimTypes.Role);

        if(rolesClaimValue.IsError)
        {
            return AuthErrors.UserForbiddenAction;
        }

        return new CurrentUserDTO(
            Id: id,
            Roles: rolesClaimValue.Value
        );
    }

    private  ErrorOr<IReadOnlyList<string>> GetClaimValues(string claimType)
    {
        var claims = _httpContextAccesor.HttpContext!.User.Claims.ToList();

        if(claims.Count == 0)
        {
            return Error.NotFound();
        }

        return _httpContextAccesor.HttpContext!.User.Claims
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .ToList();
    }
}