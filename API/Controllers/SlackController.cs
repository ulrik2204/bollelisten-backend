using API.Models;
using Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("slack")]
public class SlackController(IGroupService groupService, IPersonService personService, IEntryService entryService)
    : ControllerBase
{
    /// <summary>
    /// Handle Slack /bolle slash command.
    /// Usage: /bolle
    /// The channel name is used as the group slug. If the group doesn't exist, it is created.
    /// </summary>
    [HttpPost("entries")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<ActionResult> HandleBolleCommand([FromForm] SlackSlashCommandRequest request)
    {
        var groupSlug = request.ChannelName?.Trim();
        if (string.IsNullOrWhiteSpace(groupSlug))
            return Ok(new SlackResponse("ephemeral", "Could not determine channel name."));

        var personName = request.Text?.Trim();
        if (string.IsNullOrWhiteSpace(personName))
            personName = request.UserName;

        if (string.IsNullOrWhiteSpace(personName))
            return Ok(new SlackResponse("ephemeral", "Could not determine person name."));

        var group = await groupService.GetGroupBySlug(groupSlug, includePeople: false);
        if (group == null)
        {
            group = await groupService.CreateGroup(groupSlug, groupSlug, description: null);
            if (group == null)
                return Ok(new SlackResponse("ephemeral", "Failed to create group."));
        }

        var person = await personService.GetPersonByName(group.Id, personName);
        if (person == null)
        {
            person = await personService.CreatePerson(personName, [group]);
        }

        var entry = await entryService.CreateEntry(person.Id, group.Id, DateTime.UtcNow, fulfilledTime: null);
        if (entry == null)
            return Ok(new SlackResponse("ephemeral", "Failed to create entry."));

        return Ok(new SlackResponse("in_channel",
            $"\ud83d\udfe2 {person.Name} skylder en bolle til {group.Name}!\n\nSe hele listen: https://bollelisten.rosby.no/groups/{group.Slug}"));
    }
}
