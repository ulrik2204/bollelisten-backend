using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

public interface IEntryService
{
    public Task<Entry?> UpdateEntryById(Guid entryId, DateTime? incidentTime, DateTime? fulfilledTime);
    public Task<Entry?> GetEntryById(Guid entryId);
    public Task<List<Entry>> GetGroupEntries(Guid groupId, int? limit = null, int? offset = null);
    public Task<Entry?> CreateEntry(Guid personId, Guid groupId, DateTime incidentTime, DateTime? fulfilledTime);
    public Task<bool> DeleteEntryById(Guid entryId);
}

public class EntryService(AppDbContext dbContext) : IEntryService
{
    public async Task<Entry?> GetEntryById(Guid entryId)
    {
        return await dbContext.Entries.FirstOrDefaultAsync(e => e.Id == entryId);
    }

    public async Task<List<Entry>> GetGroupEntries(Guid groupId, int? limit = null, int? offset = null)
    {
        var query = dbContext.Entries.Where(entry => entry.GroupId == groupId)
            .Include(entry => entry.Person)
            .OrderBy(entry => entry.IncidentTime)
            as IQueryable<Entry>;
        if (offset.HasValue && offset.Value > 0)
            query = query.Skip(offset.Value);
        if (limit.HasValue && limit.Value > 0)
            query = query.Take(limit.Value);
        var entries = await query.ToListAsync();

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

    public async Task<Entry?> UpdateEntryById(Guid entryId, DateTime? incidentTime, DateTime? fulfilledTime)
    {
        var entry = await dbContext.Entries.FirstOrDefaultAsync(e => e.Id == entryId);
        if (entry == null) return null;

        // Verify that the person exists
        if (incidentTime != null) entry.IncidentTime = incidentTime.Value;
        if (fulfilledTime is not null) entry.FulfilledTime = fulfilledTime.Value;

        await dbContext.SaveChangesAsync();
        return entry;
    }

    public async Task<bool> DeleteEntryById(Guid entryId)
    {
        var entry = await dbContext.Entries.FirstOrDefaultAsync(e => e.Id == entryId);
        if (entry == null) return false;

        dbContext.Entries.Remove(entry);
        await dbContext.SaveChangesAsync();
        return true;
    }
}