using API.Models;
using Data.Entities;

namespace API.Extensions;

public static class GroupExtensions
{
    public static GroupDto ToGroupDto(this Group group)
    {
        return new GroupDto(group.Slug, group.Name);
    }
}