namespace Data.Entities;

public record Entry
{
    public required Guid Id { get; set; }
    public required DateTime IncidentTime { get; set; }
    public required Person Person { get; set; }
    public required DateTime? FulfilledTime { get; set; }
}