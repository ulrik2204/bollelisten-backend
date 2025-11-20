using Data;
using Data.Entities;

namespace Common.Services;

public interface IPersonService
{


    public Task<Person?> GetPerson(Guid id);
    public Task<Person> CreatePerson(string name, List<Group>? groups = null);

}

public class PersonService(AppDbContext dbContext) : IPersonService
{
    public async Task<Person?> GetPerson(Guid id)
    {
        var person = await dbContext.People.FindAsync(id);
        return person;
    }


    public async Task<Person> CreatePerson(string name, List<Group>? groups = null)
    {

        var person = new Person()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Groups = groups ?? []
        };
        await dbContext.People.AddAsync(person);
        await dbContext.SaveChangesAsync();
        return person;
    }

}

