using API.Extensions;
using Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupsController(IGroupService groupService) : ControllerBase
{
    /// <summary>
    /// Get a group by its slug
    /// </summary>
    /// <param name="groupSlug">The unique slug identifying the group</param>
    /// <returns>The group information</returns>
    /// <response code="200">Returns the group information</response>
    /// <response code="404">Group not found</response>
    [HttpGet("{groupSlug}")]
    public async Task<ActionResult> GetGroup(string groupSlug)
    {
        var group = await groupService.GetGroupBySlug(groupSlug);
        if (group == null) return NotFound();
        return Ok(group.ToDto());
    }

    /// <summary>
    /// Create a new group
    /// </summary>
    /// <remarks>
    /// Use it to register a new group. After creating a group, use its slug to access group-scoped resources.
    /// </remarks>
    /// <param name="groupItem">The group information to create</param>
    /// <returns>The created group information</returns>
    /// <response code="201">Group created successfully</response>
    [HttpPost]
    public async Task<ActionResult> CreateGroup([FromBody] CreateGroupRequest groupItem)
    {
        var group = await groupService.CreateGroup(groupItem.Slug, groupItem.Name, groupItem.Description);
        if (group == null) throw new Exception("Group was not created");
        return Created($"/groups/{group.Slug}", group.ToDto());
    }
}

public record CreateGroupRequest(string Slug, string Name, string? Description);
