using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

public interface IGroupService
{
    public Task<Group?> GetGroupBySlug(string slug);
    public Task<Group?> GetGroupById(string id);
    public Task<Group?> CreateGroup(string slug, string name, string? description);
}

public class GroupService(AppDbContext context) : IGroupService
{
    public async Task<Group?> GetGroupBySlug(string slug)
    {
        return await context.Groups.Include(group => group.People).SingleOrDefaultAsync(group => group.Slug == slug);
    }

    public async Task<Group?> GetGroupById(string id)
    {
        return await context.Groups.Include(group => group.People)
            .SingleOrDefaultAsync(group => group.Id.ToString() == id);
    }

    public async Task<Group?> CreateGroup(string slug, string name, string? description)
    {
        var group = new Group
        {
            Id = Guid.NewGuid(),
            Slug = slug,
            Name = name,
            Description = description,
        };
        await context.Groups.AddAsync(group);
        await context.SaveChangesAsync();
        var createdGroup = await GetGroupBySlug(slug);
        return createdGroup;
    }
}