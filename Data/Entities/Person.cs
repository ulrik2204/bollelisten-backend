using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

public class Person
{
    public required Guid Id { get; set; }
    [StringLength(128)]
    public required string Name { get; set; }
    public required DateTime CreatedAt { get; set; }
}