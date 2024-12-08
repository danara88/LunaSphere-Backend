using System.Net;

namespace LunaSphere.Api.Responses;

/// <summary>
/// Common API response
/// </summary>
public class ApiResponse<T>
{
    public T Data { get; set; }
    
    public int Status { get; set; }

    public bool Success { get; set; } = true;

    public ApiResponse(T data, HttpStatusCode status = HttpStatusCode.OK)
    {
        Data = data;
        Status = (int)status;
    }
}
