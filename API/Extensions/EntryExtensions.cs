using API.Models;
using Data.Entities;

namespace API.Extensions;

public static class EntryExtensions
{
    public static EntryDto ToDto(this Entry entry)
    {
        return new EntryDto(
            Id: entry.Id,
            person: entry.Person.ToDto(),
            group: entry.Group.ToGroupDto(),
            IncidentTime: entry.IncidentTime,
            FulfilledTime: entry.FulfilledTime
        );
    }

}