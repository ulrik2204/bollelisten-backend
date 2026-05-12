using API.Extensions;
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
    /// Usage: /bolle &lt;groupSlug&gt; [personName]
    /// If personName is omitted, uses the Slack user's username.
    /// </summary>
    [HttpPost("bolle")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<ActionResult> HandleBolleCommand([FromForm] SlackSlashCommandRequest request)
    {
        var parts = (request.Text ?? "").Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 0)
            return Ok(new SlackResponse("ephemeral", "Usage: /bolle <group> [name]"));

        var groupSlug = parts[0];
        var personName = parts.Length > 1 ? parts[1].Trim() : request.UserName;

        if (string.IsNullOrWhiteSpace(personName))
            return Ok(new SlackResponse("ephemeral", "Could not determine person name."));

        var group = await groupService.GetGroupBySlug(groupSlug, includePeople: false);
        if (group == null)
            return Ok(new SlackResponse("ephemeral", $"Group \"{groupSlug}\" not found."));

        // Look up person by name, or create them if they don't exist in the group
        var person = await personService.GetPersonByName(group.Id, personName);
        if (person == null)
        {
            person = await personService.CreatePerson(personName, [group]);
        }

        var entry = await entryService.CreateEntry(person.Id, group.Id, DateTime.UtcNow, fulfilledTime: null);
        if (entry == null)
            return Ok(new SlackResponse("ephemeral", "Failed to create entry."));

        return Ok(new SlackResponse("in_channel",
            $"\ud83d\udfe2 {person.Name} got a bolle in {group.Name ?? groupSlug}!"));
    }
}
