using API.Extensions;
using API.Models;
using Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("groups/{groupSlug}/[controller]")]
public class EntriesController(IGroupService groupService, IEntryService entryService) : ControllerBase
{

    [HttpGet("{entryId}")]
    public async Task<ActionResult> GetEntry(string groupSlug, Guid entryId)
    {
        var group = await groupService.GetGroupBySlug(groupSlug, includePeople: false);
        if (group == null) return NotFound("Group not found");

        var entry = await entryService.GetEntryById(entryId);
        if (entry is null) return NotFound();
        return Ok(entry.ToDto());

    }

    /// <summary>
    /// Get entries for a group
    /// </summary>
    /// <param name="groupSlug">The unique slug identifying the group</param>
    /// <param name="limit">Maximum number of entries to return</param>
    /// <param name="offset">Number of entries to skip</param>
    /// <returns>List of entries for the group</returns>
    /// <response code="200">Returns the list of entries</response>
    /// <response code="404">Group not found</response>
    [HttpGet]
    public async Task<ActionResult> GetEntries(string groupSlug, [FromQuery(Name = "limit")] int? limit,
        [FromQuery(Name = "offset")] int? offset)
    {
        var group = await groupService.GetGroupBySlug(groupSlug, includePeople: false);
        if (group == null) return NotFound("Group not found");
        var entries = await entryService.GetGroupEntries(group.Id, limit, offset);
        return Ok(entries.Select(entry => entry.ToDto()));
    }

    [HttpPost]
    public async Task<ActionResult> CreateEntry(string groupSlug, [FromBody] CreateEntryRequest request)
    {
        var group = await groupService.GetGroupBySlug(groupSlug, includePeople: false);
        if (group == null) return NotFound("Group not found");

        if (!Guid.TryParse(request.PersonId, out var personId))
            return BadRequest("personId is in the from Guid format");
        var entry = await entryService.CreateEntry(personId, group.Id, request.IncidentTime, request.FulfilledTime);
        if (entry == null) return BadRequest();
        return Created($"/groups/{groupSlug}/entries/{entry.Id}", entry.ToDto());
    }

    [HttpPut("{entryId}")]
    public async Task<ActionResult> UpdateEntry(string groupSlug, Guid entryId, [FromBody] UpdateEntryRequest request)
    {
        var group = await groupService.GetGroupBySlug(groupSlug, includePeople: false);
        if (group == null) return NotFound("Group not found");

        var entry = await entryService.UpdateEntryById(entryId, request.IncidentTime, request.FulfilledTime);
        if (entry == null) return NotFound();
        return Ok(entry.ToDto());
    }

    [HttpDelete("{entryId}")]
    public async Task<ActionResult> DeleteEntry(string groupSlug, Guid entryId)
    {
        var group = await groupService.GetGroupBySlug(groupSlug, includePeople: false);
        if (group == null) return NotFound("Group not found");

        var deleted = await entryService.DeleteEntryById(entryId);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
