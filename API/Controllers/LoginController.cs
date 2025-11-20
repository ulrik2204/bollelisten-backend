using API.Extensions;
using API.Services;
using Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[ApiController]
[Route("[controller]")]
public class LoginController(ISoftAuthService softAuthService): ControllerBase
{

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
