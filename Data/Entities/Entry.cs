namespace Data.Entities;

public record Entry
{
    public required Guid Id { get; set; }
    public required DateTime IncidentTime { get; set; }
    public required Guid PersonId { get; set; }
    public Person Person { get; set; } = null!;
    public required Guid GroupId { get; set; }
    public Group Group { get; set; } = null!;
    public required DateTime? FulfilledTime { get; set; }
}