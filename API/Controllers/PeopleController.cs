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

    [HttpPost]
    public async Task<ActionResult> CreatePerson([FromBody] CreatePersonRequest request)
    {
        var group = await softAuthService.GetAuthenticatedGroup();
        if (group == null) return Unauthorized();

        await personService.CreatePerson(request.Name, [group]);
        return Created();
    }

}