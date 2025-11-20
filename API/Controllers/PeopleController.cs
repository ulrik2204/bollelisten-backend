using API.Extensions;
using API.Models;
using API.Services;
using Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[ApiController]
[Route("[controller]")]
public class PeopleController(ISoftAuthService softAuthService, IPersonService personService): ControllerBase
{

    [HttpGet("{personId}")]
    public async Task<ActionResult> GetPerson(Guid personId)
    {
        var person = await personService.GetPerson(personId);
        if (person == null) return NotFound();
        return Ok(person);
    }


    [HttpPost]
    public async Task<ActionResult> CreatePerson([FromBody] CreatePersonRequest request)
    {
        var group = await softAuthService.GetAuthenticatedGroup();
        if (group == null) return Unauthorized();

        var person = await personService.CreatePerson(request.Name, [group]);
        return Created($"/people/{person.Id}", person);
    }


}