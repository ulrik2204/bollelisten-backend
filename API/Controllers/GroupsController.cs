using API.Extensions;
using Common.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class GroupsController(IGroupService groupService): ControllerBase
{

    [HttpGet("/{slug}")]
    public async Task<ActionResult> GetGroupBySlug(string slug)
    {
        var group = await groupService.GetGroupBySlug(slug);
        if (group == null) return NotFound();
        return Ok(group.ToGroupDto());
    }


    [HttpPost]
    public async Task<ActionResult> CreateGroup([FromBody] CreateGroupRequest groupItem)
    {
        var group = await groupService.CreateGroup(groupItem.Slug, groupItem.Name, groupItem.Description);
        if (group == null) throw new Exception("Group was not created");
        return Ok(group.ToGroupDto());
    }
}

public record CreateGroupRequest(string Slug, string Name, string? Description);