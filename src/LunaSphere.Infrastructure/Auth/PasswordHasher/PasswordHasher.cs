using System.Text.RegularExpressions;
using ErrorOr;

using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Domain.Users;

namespace LunaSphere.Infrastructure.Auth.PasswordHasher;

/// <summary>
/// Password hasher implementation
/// </summary>
public partial class PasswordHasher : IPasswordHasher
{
    private static readonly Regex PasswordRegex = StrongPasswordRegex();

    /// <summary>
    /// Method to hash a password
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public ErrorOr<string> HashPassword(string password)
    {
        return !PasswordRegex.IsMatch(password)
            ? AuthErrors.WeakPassword
            : BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    /// <summary>
    /// Method to compare if the hashed password is equal to the input password
    /// </summary>
    /// <param name="password"></param>
    /// <param name="hash"></param>
    /// <returns></returns>
    public bool IsCorrectPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }

    /// <summary>
    /// Gets a strong password regex
    /// At least one upper case letter
    /// At least one lower case letter
    /// At least one digit
    /// At least one special character
    /// Minimum 8 characters long
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", RegexOptions.Compiled)]
    private static partial Regex StrongPasswordRegex();
}