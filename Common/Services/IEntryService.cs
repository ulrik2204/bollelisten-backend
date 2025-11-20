using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

public interface IEntryService
{
    public Task<List<Entry>> GetGroupEntries(Guid groupId, int limit = 0, int offset = 0);
}

public class EntryService(AppDbContext dbContext) : IEntryService
{
    public async Task<List<Entry>> GetGroupEntries(Guid groupId, int limit = 0, int offset = 0)
    {
        var entries = await dbContext.Entries.
            Where(entry => entry.Group.Id == groupId)
            .Include(entry => entry.Person)
            .OrderBy(entry => entry.IncidentTime)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
        return entries;
    }
}