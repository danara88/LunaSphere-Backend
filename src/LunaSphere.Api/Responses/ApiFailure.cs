namespace LunaSphere.Api.Responses;

/// <summary>
/// Api failure API response
/// </summary>
 public class ApiFailure
{
    public string Detail { get; set; } = string.Empty;

    public int Status { get; set; }

    public bool Success { get; set; }
}