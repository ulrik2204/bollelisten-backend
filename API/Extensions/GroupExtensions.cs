using API.Models;
using Data.Entities;

namespace API.Extensions;

public static class GroupExtensions
{
    public static GroupDto ToDto(this Group group)
    {
        return new GroupDto(group.Slug, group.Name);
    }

    public static GroupDto ToDtoWithPeople(this Group group)
    {
        var people = group.People.Select(p => p.ToDto()).ToList();
        return new GroupDtoWithPeople(group.Slug, group.Name, people);
    }
}