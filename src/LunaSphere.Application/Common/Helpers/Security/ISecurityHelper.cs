namespace LunaSphere.Application.Common.Helpers;

/// <summary>
/// Security helper interface
/// </summary>
public interface ISecurityHelper
{
    string EncryptString(string inputText);

    string DecryptString(string encryptedText);
}

