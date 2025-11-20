using API.Models;
using Data.Entities;

namespace API.Extensions;

public static class EntryExtensions
{
    public static EntryDto ToDto(this Entry entry)
    {
        return new EntryDto(
            Id: entry.Id,
            Person: entry.Person.ToDto(),
            IncidentTime: entry.IncidentTime,
            FulfilledTime: entry.FulfilledTime
        );
    }

}