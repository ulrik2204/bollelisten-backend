using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

public interface IEntryService
{
    public Task<List<Entry>> GetGroupEntries(Guid groupId, int limit = 0, int offset = 0);
    public Task<Entry?> CreateEntry(Guid personId, Guid groupId, DateTime incidentTime, DateTime? fulfilledTime);
}

public class EntryService(AppDbContext dbContext) : IEntryService
{
    public async Task<List<Entry>> GetGroupEntries(Guid groupId, int limit = 0, int offset = 0)
    {
        var entries = await dbContext.Entries.Where(entry => entry.GroupId == groupId)
            .Include(entry => entry.Person)
            .OrderBy(entry => entry.IncidentTime)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
        return entries;
    }

    public async Task<Entry?> CreateEntry(Guid personId, Guid groupId, DateTime incidentTime, DateTime? fulfilledTime)
    {
        // Optionally verify that the person and group exist
        var groupExists = await dbContext.Groups.AnyAsync(g => g.Id == groupId);
        var personExists = await dbContext.People.AnyAsync(p => p.Id == personId);
        if (!groupExists || !personExists) return null;

        var entry = new Entry()
        {
            Id = Guid.NewGuid(),
            GroupId = groupId,
            PersonId = personId,
            IncidentTime = incidentTime,
            FulfilledTime = fulfilledTime,
        };
        await dbContext.Entries.AddAsync(entry);
        await dbContext.SaveChangesAsync();
        return entry;
    }
}