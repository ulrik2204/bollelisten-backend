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
        // Skip certain paths (like login or static files)
        if (context.Request.Path.StartsWithSegments("/login"))
        {
            await _next(context);
            return;
        }

        // Check if cookie exists
        var sessionId = context.GetSessionId();
        if (sessionId == null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: missing X-SessionId header.");
            return;

        }

        var groupKey = context.GetGroupKeyCookie();
        if (groupKey == null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: missing authentication cookie.");
            return;
        }

        if (!softAuthService.IsAuthenticated(sessionId, groupKey))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: group not found.");
            return;
        }


        // Otherwise, continue the request
        await _next(context);
    }
}
