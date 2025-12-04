using API.Extensions;
using API.Services;
using Common.Services;
using Microsoft.Extensions.Caching.Memory;

namespace API.Middleware;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class CookieAuthMiddleware
{
    private readonly RequestDelegate _next;

    public CookieAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ISoftAuthService softAuthService)
    {
        // Always allow OPTIONS requests to pass through for CORS
        if (context.Request.Method == "OPTIONS")
        {
            await _next(context);
            return;
        }
        // Skip certain paths (like login, swagger, or static files)
        if (context.Request.Path.StartsWithSegments("/login") ||
            (context.Request.Path.StartsWithSegments("/groups") && context.Request.Method == "POST") ||
            context.Request.Path.StartsWithSegments("/swagger") ||
            context.Request.Path.StartsWithSegments("/scalar") ||
            context.Request.Path.StartsWithSegments("/openapi"))
        {
            await _next(context);
            return;
        }
        Console.WriteLine("SessionId: " + context.GetSessionId());
        Console.WriteLine("groupKey: " + context.GetGroupKeyCookie());

        if (!softAuthService.IsAuthenticated())
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync(
                "Unauthorized: Invalid or missing authentication. Required both the X-SessionId header and the groupKey cookie");
            return;
        }


        // Otherwise, continue the request
        await _next(context);
    }
}