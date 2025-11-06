using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class EntriesController : ControllerBase
{

    public EntriesController()
    {
        // Constructor logic can be added here if needed.
    }

    [HttpGet]
    public async Task<ActionResult<List<string>>> GetEntries()
    {
        // This is a placeholder for the actual implementation.
        // You would typically fetch entries from a database or another service.
        var entries = new List<string> { "Entry1", "Entry2", "Entry3" };

        return entries;
    }

}