namespace API.Models;

public record GroupDto(
    string Slug,
    string Name
);

public record GroupDtoWithPeople(string Slug, string Name, List<PersonDto> People): GroupDto(Slug, Name);