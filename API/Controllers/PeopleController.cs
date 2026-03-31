using API.Extensions;
using API.Models;
using Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[ApiController]
[Route("groups/{groupSlug}/[controller]")]
public class PeopleController(IGroupService groupService, IPersonService personService) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult> GetPeople(string groupSlug)
    {
        var group = await groupService.GetGroupBySlug(groupSlug, includePeople: false);
        if (group == null) return NotFound("Group not found");

        var people = await personService.GetPeople(group.Id);
        return Ok(people.Select(person => person.ToDto()).ToList());

    }

    [HttpGet("{personId}")]
    public async Task<ActionResult> GetPerson(string groupSlug, Guid personId)
    {
        var person = await personService.GetPerson(personId);
        if (person == null) return NotFound();
        return Ok(person.ToDto());
    }


    [HttpPost]
    public async Task<ActionResult> CreatePerson(string groupSlug, [FromBody] CreatePersonRequest request)
    {
        var group = await groupService.GetGroupBySlug(groupSlug, includePeople: false);
        if (group == null) return NotFound("Group not found");

        var person = await personService.CreatePerson(request.Name, [group]);
        return Created($"/groups/{groupSlug}/people/{person.Id}", person.ToDto());
    }


}
