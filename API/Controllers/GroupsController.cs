using API.Extensions;
using API.Services;
using Common.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupsController(IGroupService groupService, ISoftAuthService softAuthService): ControllerBase
{
    /// <summary>
    /// Get the currently authenticated group
    /// </summary>
    /// <remarks>
    /// **Authentication Required**: This endpoint requires both the X-SessionId header and the groupKey cookie.
    /// Call POST /login first to obtain the authentication cookie.
    /// </remarks>
    /// <returns>The currently authenticated group information</returns>
    /// <response code="200">Returns the group information</response>
    /// <response code="401">Authentication failed or missing credentials</response>
    [HttpGet("current")]
    public async Task<ActionResult> GetCurrentGroup()
    {
        var group = await softAuthService.GetAuthenticatedGroup();
        if (group == null) return Unauthorized();
        return Ok(group.ToDtoWithPeople());
    }

    /// <summary>
    /// Create a new group
    /// </summary>
    /// <remarks>
    /// This endpoint does not require authentication. Use it to register a new group.
    /// After creating a group, call POST /login with the group slug to authenticate.
    /// </remarks>
    /// <param name="groupItem">The group information to create</param>
    /// <returns>The created group information</returns>
    /// <response code="200">Group created successfully</response>
    [HttpPost]
    public async Task<ActionResult> CreateGroup([FromBody] CreateGroupRequest groupItem)
    {
        var group = await groupService.CreateGroup(groupItem.Slug, groupItem.Name, groupItem.Description);
        if (group == null) throw new Exception("Group was not created");
        return Created((string?)null, group.ToDtoWithPeople());
    }
}

public record CreateGroupRequest(string Slug, string Name, string? Description);