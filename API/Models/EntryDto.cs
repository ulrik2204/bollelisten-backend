namespace API.Models;

public record EntryDto(Guid Id, PersonDto person, GroupDto group, DateTime IncidentTime, DateTime? FulfilledTime);

public record CreateEntryRequest(string PersonId, DateTime IncidentTime, DateTime? FulfilledTime);