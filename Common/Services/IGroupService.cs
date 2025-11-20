using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

public interface IGroupService
{

    public Task<Group?> GetGroupBySlug(string slug);
    public Task<Group?> GetGroupById(string id);

}

public class GroupService(AppDbContext context) : IGroupService
{


    public async Task<Group?> GetGroupBySlug(string slug)
    {
        return await context.Groups.SingleOrDefaultAsync(group => group.Slug == slug);
    }

    public async Task<Group?> GetGroupById(string id)
    {
        return await context.Groups.SingleOrDefaultAsync(group => group.Id.ToString() == id);
    }
}