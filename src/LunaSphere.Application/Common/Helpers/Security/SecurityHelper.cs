using Microsoft.AspNetCore.DataProtection;

namespace LunaSphere.Application.Common.Helpers;

/// <summary>
/// Security helper. 
/// In charge of dealing with security system purposes.
/// </summary>
public class SecurityHelper : ISecurityHelper
{
    private readonly IDataProtector _protector;

    public SecurityHelper(IDataProtectionProvider dataProtectionProvider)
    {
        _protector = dataProtectionProvider.CreateProtector("LunaSphere.Security.Encryption");
    }

    public string EncryptString(string inputText)
    {
        return _protector.Protect(inputText);
    }

    public string DecryptString(string encryptedText)
    {
        return _protector.Unprotect(encryptedText);
    }
}
