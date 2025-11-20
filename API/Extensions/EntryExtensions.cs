using API.Models;
using Data.Entities;

namespace API.Extensions;

public static class EntryExtensions
{
    public static EntryDto ToDto(this Entry entry)
    {
        return new EntryDto(
            PersonName: entry.Person.Name,
            IncidentTime: entry.IncidentTime,
            FulfilledTime: entry.FulfilledTime
        );
    }

}