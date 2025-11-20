using API.Models;
using Data.Entities;

namespace API.Extensions;

public static class PersonExtensions
{

    public static PersonDto ToDto(this Person person)
    {
        return new PersonDto(
            person.Id,
            person.Name
        );

}