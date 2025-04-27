using LunaSphere.Infrastructure.Common.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace LunaSphere.Infrastructure;

public static class RequestPipeline 
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<EventualConsistencyMiddleware>();
        
        return builder;
    }
}