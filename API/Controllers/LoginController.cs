using API.Extensions;
using API.Services;
using Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[ApiController]
[Route("[controller]")]
public class LoginController(ISoftAuthService softAuthService): ControllerBase
{
    /// <summary>
    /// Authenticate a group and receive authentication cookie
    /// </summary>
    /// <remarks>
    /// Call this endpoint first to authenticate. You must include the X-SessionId header.
    ///
    /// Upon successful authentication, a groupKey cookie will be set in the response.
    /// Include both the X-SessionId header and the groupKey cookie in all subsequent requests.
    /// </remarks>
    /// <param name="groupLoginRequest">The group login credentials</param>
    /// <returns>Success if authentication is successful</returns>
    /// <response code="200">Authentication successful, cookie has been set</response>
    /// <response code="401">Missing X-SessionId header or group not found</response>
    [HttpPost]
    public async Task<ActionResult> Login(GroupLoginRequest groupLoginRequest)
    {
        var sessionId = HttpContext.GetSessionId();
        if (sessionId == null) return Unauthorized("Missing X-SessionId header in request");
        var groupKey = await softAuthService.Authenticate(groupLoginRequest.GroupSlug);
        if (groupKey == null) return Unauthorized("Group with given slug not found");
        // Using the Set-Cookie header to set the cookie on the client
        HttpContext.SetGroupKeyCookie(groupKey);
        return Ok();
    }

}

public record GroupLoginRequest(string GroupSlug);
