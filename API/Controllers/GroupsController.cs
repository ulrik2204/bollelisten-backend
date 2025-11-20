using API.Extensions;
using API.Services;
using Common.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class GroupsController(IGroupService groupService, ISoftAuthService softAuthService): ControllerBase
{

    [HttpGet("current")]
    public async Task<ActionResult> GetCurrentGroup()
    {
        var group = await softAuthService.GetAuthenticatedGroup();
        if (group == null) return Unauthorized();
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