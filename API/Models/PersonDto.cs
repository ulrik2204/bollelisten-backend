using EnvironmentName = Microsoft.AspNetCore.Hosting.EnvironmentName;

namespace API.Models;

public record PersonDto(Guid Id, string Name);