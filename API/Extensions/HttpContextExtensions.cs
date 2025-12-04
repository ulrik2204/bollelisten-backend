namespace API.Extensions;

public static class HttpContextExtensions
{
    private const string GroupCookieKey = "groupKey";

    public static string? GetSessionId(this HttpContext context)
    {
        return context.Request.Headers["X-SessionId"];
    }


    public static string? GetGroupKeyCookie(this HttpContext context)
    {
        if (context.Request.Cookies.TryGetValue(GroupCookieKey, out var value))
            return value;

        return null;
    }

    public static void SetGroupKeyCookie(this HttpContext context, string groupKey)
    {
        context.Response.Cookies.Append(
            GroupCookieKey,
            groupKey,
            new CookieOptions
            {
                HttpOnly = true,         // Prevents JavaScript access
                Secure = false,           // Ensures it’s sent only over HTTPS
                SameSite = SameSiteMode.Lax, // Adjust as needed
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });
    }

}