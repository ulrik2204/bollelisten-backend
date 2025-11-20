using System.Linq.Expressions;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

public interface IGroupService
{
    public Task<Group?> GetGroupBySlug(string slug, bool includePeople = true);
    public Task<Group?> GetGroupById(Guid id, bool includePeople = true);
    public Task<Group?> CreateGroup(string slug, string name, string? description);
}

public class GroupService(AppDbContext context) : IGroupService
{
    public async Task<Group?> GetGroupBySlug(string slug, bool includePeople = true)
    {
        return await GetGroupAsync(query => query.Slug == slug, includePeople);
    }

    public async Task<Group?> GetGroupById(Guid id, bool includePeople = true)
    {
        return await GetGroupAsync(query => query.Id == id, includePeople);
    }

    private async Task<Group?> GetGroupAsync(Expression<Func<Group, bool>> predicate, bool includePeople)
    {
        var query = context.Groups.AsQueryable();
        if (includePeople)
        {
            query = query.Include(group => group.People);
        }
        return await query.SingleOrDefaultAsync(predicate);
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