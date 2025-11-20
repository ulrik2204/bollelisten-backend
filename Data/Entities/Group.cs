using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Index(nameof(Slug),  IsUnique = true)]
public class Group
{
    public required Guid Id { get; set; }
    [StringLength(8, MinimumLength = 3)]
    public required string Slug { get; set; }
    [StringLength(128)]
    public required string Name { get; set; }
    [StringLength(256)]
    public string? Description { get; set; }
    public required List<Person> Users { get; set; }
    public DateTime CreatedAt { get; set; }

}