using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

public class Person
{
    public required Guid Id { get; set; }
    [StringLength(128)]
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Group> Groups { get; set; } = [];
    public List<Entry> Entries { get; set; } = [];
}