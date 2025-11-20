using API.Extensions;
using API.Services;
using Common.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class EntriesController(ISoftAuthService softAuthService, IEntryService entryService) : ControllerBase
{
    /// <summary>
    /// Get entries for the authenticated group
    /// </summary>
    /// <remarks>
    /// **Authentication Required**: This endpoint requires both the X-SessionId header and the groupKey cookie.
    /// Call POST /login first to obtain the authentication cookie.
    /// </remarks>
    /// <param name="limit">Maximum number of entries to return</param>
    /// <param name="offset">Number of entries to skip</param>
    /// <returns>List of entries for the authenticated group</returns>
    /// <response code="200">Returns the list of entries</response>
    /// <response code="401">Authentication failed or missing credentials</response>
    [HttpGet]
    public async Task<ActionResult> GetEntries([FromQuery(Name = "limit")] int limit, [FromQuery(Name = "offset")] int offset)
    {
        // This is a placeholder for the actual implementation.
        // You would typically fetch entries from a database or another service.
        var group = await softAuthService.GetAuthenticatedGroup();
        if (group == null) return Unauthorized();
        var entries = await entryService.GetGroupEntries(group.Id, limit, offset);
        return Ok(entries.Select(entry => entry.ToDto()));
    }

}