using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;

using LunaSphere.Domain.Exceptions;
using LunaSphere.Api.Responses;

namespace LunaSphere.Infrastructure.Filters;

/// <summary>
/// Implements exception filter globally
/// </summary>
public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        OnInternalServerException(context);
    }

    /// <summary>
    /// Configures the internal server exception
    /// </summary>
    /// <param name="context"></param>
    private void OnInternalServerException(ExceptionContext context)
    {
        if (context.Exception.GetType() == typeof(InternalServerException))
        {
            var exception = (InternalServerException)context.Exception;

            var error = new ApiFailure
            {
                Title = "Internal Server Error",
                Detail = exception.Message,
                Status = (int)HttpStatusCode.InternalServerError,
                Success = false
            };

            context.Result = new ObjectResult(error);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.ExceptionHandled = true;
        }
    }
}