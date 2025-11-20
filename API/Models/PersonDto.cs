namespace API.Models;

public record PersonDto(Guid Id, string Name);

public record CreatePersonRequest(string Name);